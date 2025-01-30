using System.Net;
using System.Security.Claims;
using Cortside.Authorization.Client.Models;
using Cortside.Authorization.Client.Models.Requests;
using Cortside.Authorization.Client.Models.Responses;
using Cortside.Common.Messages.MessageExceptions;
using Cortside.Common.Validation;
using Cortside.RestApiClient;
using Cortside.RestApiClient.Authenticators.OpenIDConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RestSharp;

namespace Cortside.Authorization.Client {
    public class AuthorizationApiClient : IAuthorizationApiClient {
        private readonly ILogger<AuthorizationApiClient> logger;
        private readonly AuthorizationApiClientConfiguration config;
        private readonly JsonNetSerializer serializer;
        private readonly MemoryDistributedCache memCache;
        private readonly RestApiClient.RestApiClient client;
        private readonly AsyncRetryPolicy<RestResponse> defaultRetryPolicy;

        public AuthorizationApiClient(AuthorizationApiClientConfiguration config, ILogger<AuthorizationApiClient> logger, IHttpContextAccessor context, RestApiClientOptions options = null) {
            this.logger = logger;
            this.config = config;
            this.serializer = new JsonNetSerializer();
            memCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            options ??= new RestApiClientOptions {
                BaseUrl = new Uri(config.ServiceUrl),
                Authenticator = new OpenIDConnectAuthenticator(context, config.Authentication)
                    .UsePolicy(PolicyBuilderExtensions.Handle<Exception>()
                        .OrResult(r => r.StatusCode == HttpStatusCode.Unauthorized || r.StatusCode == 0)
                        .WaitAndRetryAsync(PolicyBuilderExtensions.Jitter(1, 2))
                    )
                    .UseLogger(logger),
                Serializer = serializer,
                Cache = memCache
            };
            client = new RestApiClient.RestApiClient(logger, context, options);
            defaultRetryPolicy = PolicyBuilderExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutException>()
                    .OrResult(x => x.StatusCode == 0 || x.StatusCode == System.Net.HttpStatusCode.Unauthorized || x.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    .WaitAndRetryAsync(PolicyBuilderExtensions.Jitter(1, 3));
        }

        public async Task<EvaluationResponse> EvaluatePolicyAsync(EvaluationDto dto) {
            if (dto.User != null && !(dto.User.Identity?.IsAuthenticated ?? false)) {
                logger.LogInformation("Policy evaluation request for anonymous user, returning no results.");
                return new EvaluationResponse();
            }

            var requestModel = Map(dto);

            logger.LogInformation("Evaluating policy {PolicyResourceId} for subject {SubjectId}", config.PolicyResourceId, dto.User?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);

            if (config.CacheEnabled) {
                var cached = await memCache.GetValueAsync<EvaluationResponse>(requestModel.GetCacheKey(config.PolicyResourceId), serializer);
                if (cached != null) {
                    logger.LogInformation("Returning cached policy evaluation result");
                    return cached;
                }
            }

            RestApiRequest request = new RestApiRequest($"api/v1/policies/{config.PolicyResourceId}/evaluate", Method.Post) {
                Policy = defaultRetryPolicy
            };
            request.AddJsonBody(requestModel);

            var response = await client.ExecuteAsync<EvaluationResponse>(request).ConfigureAwait(false);
            if (!response.IsSuccessful) {
                logger.LogError(response.ErrorException, "Failure evaluating authorization policy: {Message}", response.ErrorMessage);
                Guard.Against(() => response.StatusCode == HttpStatusCode.NotFound, new NotFoundResponseException($"Policy not found {config.PolicyResourceId}"));
                Guard.Against(() => response.StatusCode == HttpStatusCode.BadRequest, new BadRequestResponseException($"Bad Request: {response.Content}"));
                throw response.LoggedFailureException(logger, "Error contacting authorization api to retrieve item info for {0}", config.PolicyResourceId);
            }

            if (config.CacheEnabled) {
                await memCache.SetValueAsync<EvaluationResponse>(requestModel.GetCacheKey(config.PolicyResourceId), response.Data, serializer, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = config.CacheDuration });
            }

            return response.Data;
        }


        public async Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionName) {
            var dto = new EvaluationDto { User = user };
            var result = await EvaluatePolicyAsync(dto).ConfigureAwait(false);
            return result.Permissions.Contains(permissionName, StringComparer.InvariantCultureIgnoreCase);
        }

        public EvaluationRequest Map(EvaluationDto dto) {
            return new EvaluationRequest {
                Claims = GetClaims(dto.User),
            };
        }

        private IEnumerable<ClaimModel> GetClaims(ClaimsPrincipal user) {
            if (config.ClaimTypes.Any()) {
                var matched = user.Claims.Where(c => config.ClaimTypes.Contains(c.Type, StringComparer.InvariantCultureIgnoreCase));
                return Map(matched);
            }
            return Map(user.Claims);
        }

        private IEnumerable<ClaimModel> Map(IEnumerable<Claim> claims) {
            return claims.Select(c => new ClaimModel {
                Type = c.Type,
                Value = c.Value,
            });
        }

    }
}

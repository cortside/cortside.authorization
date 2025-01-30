using Cortside.Common.Validation;
using Cortside.RestApiClient;
using Cortside.RestApiClient.Authenticators.OpenIDConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortside.Authorization.Client.AspNetCore {
    public static class ServiceCollectionExtensions {

        public static IServiceCollection AddAuthorizationApiClient(this IServiceCollection services, IConfiguration configuration) {
            Guard.Against(() => !configuration.GetSection("IdentityServer").Exists(), () => throw new ArgumentException("Configuration section named 'IdentityServer' is missing"));
            Guard.Against(() => !configuration.GetSection("IdentityServer:Authentication").Exists(), () => throw new ArgumentException("Configuration section 'IdentityServer:Authentication' is missing"));
            Guard.Against(() => !configuration.GetSection("IdentityServer:Authority").Exists(), () => throw new ArgumentException("Configuration section 'IdentityServer:Authority' is missing"));
            Guard.Against(() => !configuration.GetSection("AuthorizationApi").Exists(), () => throw new ArgumentException("Configuration section named 'AuthorizationApi' is missing"));

            var authorizationConfig = configuration.GetSection("AuthorizationApi").Get<AuthorizationApiClientConfiguration>();
            var tokenConfig = configuration.GetSection("IdentityServer:Authentication").Get<TokenRequest>();
            tokenConfig.AuthorityUrl = configuration.GetSection("IdentityServer:Authority").Get<string>();
            authorizationConfig.Authentication = tokenConfig;
            services.AddRestApiClient<IAuthorizationApiClient, AuthorizationApiClient, AuthorizationApiClientConfiguration>(authorizationConfig);
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

            return services;
        }
    }
}

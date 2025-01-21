using System.Security.Claims;
using Cortside.Authorization.Client.Models;
using Cortside.Authorization.Client.Models.Responses;
using Cortside.MockServer;
using Cortside.RestApiClient.Authenticators.OpenIDConnect;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace Cortside.Authorization.Client.Tests {
    public class AuthorizationApiClientTest {
        private readonly MockHttpServer server;
        private readonly Mock<ILogger<AuthorizationApiClient>> loggerMock;
        private readonly AuthorizationApiClientConfiguration config;
        private readonly AuthorizationApiClient client;

        public AuthorizationApiClientTest() {
            server = MockHttpServer.CreateBuilder(Guid.NewGuid().ToString())
                .Build();
            server.WireMockServer
                .Given(
                    Request.Create().WithPath("/connect/token").UsingPost()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(_ => JsonConvert.SerializeObject(
                            new TokenResponse {
                                AccessToken = "blah",
                                ExpiresIn = 300,
                                TokenType = "Bearer"
                            }
                        ))
                    );


            loggerMock = new Mock<ILogger<AuthorizationApiClient>>();
            var tokenRequest = new TokenRequest {
                AuthorityUrl = server.Url,
                ClientId = "consumer-api",
                ClientSecret = "secret",
                GrantType = "client_credentials",
                Scope = "authorization-api",
                SlidingExpiration = 30
            };
            config = new AuthorizationApiClientConfiguration {
                ServiceUrl = server.Url,
                CacheDuration = TimeSpan.FromSeconds(10),
                ClaimTypes = new List<string> { "sub", "groups" },
                PolicyResourceId = Guid.NewGuid(),
                Authentication = tokenRequest
            };

            client = new AuthorizationApiClient(config,
                loggerMock.Object,
                new Mock<IHttpContextAccessor>().Object
                );
        }

        [Fact]
        public async Task ShouldEvaluatePolicyNonAuthenticatedUserAsync() {
            // arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            var dto = new EvaluationDto { User = user };

            // act
            var result = await client.EvaluatePolicyAsync(dto);

            // assert
            result.Should().NotBeNull();
            result.Permissions.Should().NotBeNull().And.BeEmpty();
            result.Roles.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task ShouldEvaluatePolicyAsync() {
            // arrange
            List<Claim> claims = new List<Claim> { new Claim("sub", "jane"), new Claim("groups", "admin") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = new List<string> { "CanRead", "CanWrite" },
                Roles = new List<string> { "admin" }
            };
            server.WireMockServer
                .Given(Request.Create().WithPath($"/api/v1/policies/{config.PolicyResourceId}/evaluate")
                .UsingPost())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(response));

            // act
            var result = await client.EvaluatePolicyAsync(dto);

            // assert
            result.Should().NotBeNull();
            result.Permissions.Should().NotBeNullOrEmpty().And.Contain(response.Permissions);
            result.Roles.Should().NotBeNullOrEmpty().And.Contain(response.Roles);
        }

        [Fact]
        public async Task ShouldHavePermissionAsync() {
            // arrange
            List<Claim> claims = new List<Claim> { new Claim("sub", "jane"), new Claim("groups", "admin") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = new List<string> { "CanRead", "CanWrite" },
            };
            server.WireMockServer
                .Given(Request.Create().WithPath($"/api/v1/policies/{config.PolicyResourceId}/evaluate")
                .UsingPost())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(response));

            // act
            var result = await client.HasPermissionAsync(user, "CanWrite");

            // assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldNotHavePermissionAsync() {
            // arrange
            List<Claim> claims = new List<Claim> { new Claim("sub", "jane"), new Claim("groups", "admin") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = new List<string> { "CanRead" },
            };
            server.WireMockServer
                .Given(Request.Create().WithPath($"/api/v1/policies/{config.PolicyResourceId}/evaluate")
                .UsingPost())
                .RespondWith(Response.Create().WithStatusCode(200).WithBodyAsJson(response));

            // act
            var result = await client.HasPermissionAsync(user, "CanWrite");

            // assert
            result.Should().BeFalse();
        }
    }
}

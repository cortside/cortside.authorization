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
                                AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjY1QjBCQTk2MUI0NDMwQUJDNTNCRUI5NkVDMjBDNzQ5OTdGMDMwMzJSUzI1NiIsInR5cCI6IkpXVCIsIng1dCI6IlpiQzZsaHRFTUt2Rk8tdVc3Q0RIU1pmd01ESSJ9.eyJuYmYiOjE2NDI3OTE3ODMsImV4cCI6MTY0MjgwOTc4MywiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eXNlcnZlci5rOHMuZW5lcmJhbmsuY29tIiwiYXVkIjpbInRlcm1zY2FsY3VsYXRvci1hcGkiLCJodHRwczovL2lkZW50aXR5c2VydmVyLms4cy5lbmVyYmFuay5jb20vcmVzb3VyY2VzIl0sImNsaWVudF9pZCI6ImF1dG9tYXRpb24uc3lzdGVtMSIsInN1YiI6IjQ1OWI5NGJhLTEwMTUtNDJhNC05YzRkLTVjNThiZmZhM2E5YSIsImdyb3VwcyI6ImVjYmRhM2Y3LWQ2MjgtNGU1YS04NzljLWNmNmNhYWQyM2JhMSIsInN1YmplY3RfdHlwZSI6ImNsaWVudCIsImlhdCI6MTY0Mjc5MTc4Mywic2NvcGUiOlsidGVybXNjYWxjdWxhdG9yLWFwaSJdfQ.siCAaMAp6O9ZiWGM7c7b_U3gRkx-lb4IqfxxFyI5LBLPGB9bSHy6fl5PHaIIjs_VO0TnuVv7gjafqUTkVEbpJbf3pbQOdvzNzfGKTWgEn44dbiz0ROuDy2_qpGqAUlo1r5nkYcuDUtyLP5FsrmxSjUP0DlanuWNWSiqx5YVdzenGeLSFD59cCszZnmS2Q9KPV5MOkaUEJnd2D44UJIcccoEdRhSDkY8a6Fs03Iodf2bzvcb2mZ6aiRHhjTo2XvqiG0azLIX2W735eiSX52qoUB6ae6bDzGpEXeE3z3bRw5fomgy40XXyct64IRnIW_Hfk0ZmNyw1L51ZJwlYSF7OPA",
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
                ClaimTypes = ["sub", "groups"],
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
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("groups", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = ["CanRead", "CanWrite"],
                Roles = ["admin"]
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
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("groups", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = ["CanRead", "CanWrite"],
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
            List<Claim> claims = [new Claim("sub", "jane"), new Claim("groups", "admin")];
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            var response = new EvaluationResponse {
                Permissions = ["CanRead"],
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

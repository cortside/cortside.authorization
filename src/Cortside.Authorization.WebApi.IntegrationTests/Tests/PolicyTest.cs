using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Cortside.Authorization.Client;
using Cortside.Authorization.Client.Models;
using Cortside.Authorization.Data;
using Cortside.Authorization.WebApi.Models.Requests;
using Cortside.Authorization.WebApi.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using ClaimModel = Cortside.Authorization.WebApi.Models.ClaimModel;

namespace Cortside.Authorization.WebApi.IntegrationTests.Tests {
    public class PolicyTest : IClassFixture<IntegrationFixture> {
        private readonly string subjectId;
        private readonly HttpClient client;
        private readonly IntegrationFixture fixture;

        public PolicyTest(IntegrationFixture fixture, ITestOutputHelper output) {
            this.fixture = fixture;
            fixture.TestOutputHelper = output;
            subjectId = "132953b2-f6a7-4c1d-8da1-2b3c3dafe1c5";
            client = fixture.CreateAuthorizedClient("api");
        }

        [Fact]
        public async Task ShouldEvaluatePolicyAsync() {
            //arrange
            var request = new EvaluatePolicyRequest { Claims = [new ClaimModel { Type = "sub", Value = subjectId }] };
            var db = fixture.NewScopedDbContext<DatabaseContext>();
            var policy = await db.Policies.FirstOrDefaultAsync();

            //act
            var response = await client.PostAsJsonAsync($"api/v1/policies/{policy.PolicyResourceId}/evaluate", request);

            //assert
            var content = await response.Content.ReadAsStringAsync();
            fixture.TestOutputHelper.WriteLine($"evaluate policy response content: {content}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseObj = JsonConvert.DeserializeObject<AuthorizationModel>(content, fixture.SerializerSettings);
            responseObj.Should().NotBeNull();
            responseObj.Permissions.Should().NotBeNullOrEmpty();
            responseObj.Roles.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldUseActualClientAsync() {
            //arrange
            var config = new AuthorizationApiClientConfiguration {
                Authentication = new RestApiClient.Authenticators.OpenIDConnect.TokenRequest {
                    AuthorityUrl = fixture.Api.MockServer.WireMockServer.Url
                },
                PolicyResourceId = Guid.Parse("D7E85B2D-0399-4497-83C0-332DDC367CD3"),
                ServiceUrl = fixture.MockServer.WireMockServer.Url
            };
            var authClient = new AuthorizationApiClient(config, new Mock<ILogger<AuthorizationApiClient>>().Object, new HttpContextAccessor());

            List<Claim> claims = new List<Claim> { new Claim("sub", subjectId), new Claim("groups", "admin") };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "bearer"));
            var dto = new EvaluationDto { User = user };

            //act
            var response = await authClient.EvaluatePolicyAsync(dto);

            //assert
            response.Should().NotBeNull();
            response.Permissions.Should().NotBeNullOrEmpty();
            response.Roles.Should().NotBeNullOrEmpty();
        }
    }
}

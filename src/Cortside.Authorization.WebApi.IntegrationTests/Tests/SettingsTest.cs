using System.Net;
using System.Threading.Tasks;
using Cortside.RestApiClient;
using FluentAssertions;
using RestSharp;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.Authorization.WebApi.IntegrationTests.Tests {
    public class SettingsTest : IClassFixture<IntegrationFixture> {
        private readonly Cortside.RestApiClient.RestApiClient client;

        public SettingsTest(IntegrationFixture api, ITestOutputHelper output) {
            api.TestOutputHelper = output;
            client = api.CreateRestApiClient(output);
        }

        [Fact]
        public async Task TestAsync() {
            //arrange
            var request = new RestApiRequest("api/settings", Method.Get);

            //act
            var response = await client.GetAsync(request);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Contain("authorization-api");
        }
    }
}

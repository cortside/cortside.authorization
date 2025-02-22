using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cortside.Health.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Cortside.Authorization.WebApi.IntegrationTests.Tests {
    public class HealthTest : IClassFixture<IntegrationFixture> {
        private readonly IntegrationFixture webApi;
        private readonly HttpClient testServerClient;

        public HealthTest(IntegrationFixture webApi) {
            this.webApi = webApi;
            testServerClient = webApi.Api.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task TestAsync() {
            //arrange
            var success = false;
            HttpResponseMessage response = null;
            var timer = new Stopwatch();
            timer.Start();

            //act          
            while (!success && timer.ElapsedMilliseconds < 45000) {
                await Task.Delay(500);
                response = await testServerClient.GetAsync("api/health");
                success = response.IsSuccessStatusCode;
            }

            //assert
            var content = await response.Content.ReadAsStringAsync();
            var respObj = JsonConvert.DeserializeObject<HealthModel>(content, webApi.SerializerSettings);
            Assert.True(respObj.Healthy, content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

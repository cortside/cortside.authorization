using Cortside.Authorization.Client.AspNetCore;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cortside.Authorization.Client.Tests {
    public class ServiceCollectionExtensionsTest {

        [Fact]
        public void ShouldValidateAllConfigurationPresent() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["IdentityServer:Authentication:ClientSecret"] = "secret",
                    ["AuthorizationApi:ServiceUrl"] = "http://authorization-api"
                })
                .Build();

            services.AddAuthorizationApiClient(config);

            // act
            var built = services.BuildServiceProvider();
            var actual = built.GetRequiredService<AuthorizationApiClientConfiguration>();

            // assert
            actual.Authentication.AuthorityUrl.Should().Be("http://ids");
            actual.Authentication.ClientId.Should().Be("clientId");
            actual.Authentication.ClientSecret.Should().Be("secret");
            actual.ServiceUrl.Should().Be("http://authorization-api");
        }


        [Fact]
        public void ShouldRequireAuthorizationConfiguration() {
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    ["IdentityServer:Authority"] = "http://ids",
                    ["IdentityServer:Authentication:ClientId"] = "clientId",
                    ["IdentityServer:Authentication:ClientSecret"] = "secret",
                })
                .Build();

            // act & assert
            services.Invoking(x => x.AddAuthorizationApiClient(config)).Should().Throw<ArgumentException>()
               .WithMessage("Configuration section named 'AuthorizationApi' is missing");

        }
    }
}

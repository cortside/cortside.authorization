using Cortside.Authorization.Client.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Xunit;

namespace Cortside.Authorization.Client.Tests {
    public class PermissionProviderTest {
        [Fact]
        public async Task ShouldGetNewPolicyByPermissionName() {
            // arrange
            var authorizationOptions = Options.Create(new AuthorizationOptions());
            authorizationOptions.Value.AddPolicy("Existing", new AuthorizationPolicyBuilder()
                .RequireClaim("scope", "admin")
                .Build()
            );
            var provider = new PermissionPolicyProvider(authorizationOptions);
            var permission = "PermissionToDoSomething";

            // act
            var result = await provider.GetPolicyAsync(permission);

            // assert
            result.Should().NotBeNull();
            result.Requirements.Should().NotBeNullOrEmpty().And.HaveCount(1);
            result.Requirements.Should().Contain(r => r.GetType() == typeof(PermissionRequirement));
            var actual = result.Requirements[0] as PermissionRequirement;
            actual.PermissionName.Should().Be(permission);
        }

        [Fact]
        public async Task ShouldGetExistingPolicy() {
            // arrange
            var authorizationOptions = Options.Create(new AuthorizationOptions());
            authorizationOptions.Value.AddPolicy("Existing", new AuthorizationPolicyBuilder()
                .RequireClaim("scope", "admin")
                .Build()
            );

            var provider = new PermissionPolicyProvider(authorizationOptions);

            // act
            var result = await provider.GetPolicyAsync("Existing");

            // assert
            result.Should().NotBeNull();
            result.Requirements.Should().NotBeNullOrEmpty().And.HaveCount(1);
            result.Requirements.Should().Contain(r => r.GetType() == typeof(ClaimsAuthorizationRequirement));
        }
    }
}

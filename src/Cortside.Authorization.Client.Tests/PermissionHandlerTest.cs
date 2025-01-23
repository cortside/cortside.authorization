using System.Security.Claims;
using Cortside.Authorization.Client.AspNetCore;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Cortside.Authorization.Client.Tests {
    public class PermissionHandlerTest {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async Task ShouldHandlePermission(bool policyResponse, bool expectedSuccess) {
            // arrange
            var authApiMock = new Mock<IAuthorizationApiClient>();
            var handler = new PermissionRequirementHandler(authApiMock.Object);
            var permission = "Update";
            var requirements = new[] { new PermissionRequirement(permission) };
            var user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] {
                        new Claim("sub", Guid.NewGuid().ToString()),
                            }, "bearer")
                        );
            var resource = new DefaultHttpContext();
            var context = new AuthorizationHandlerContext(requirements, user, resource);
            authApiMock.Setup(x => x.HasPermissionAsync(user, permission)).ReturnsAsync(policyResponse)
                .Verifiable(Times.Once);

            // act
            await handler.HandleAsync(context);

            // assert
            context.HasSucceeded.Should().Be(expectedSuccess);
        }
    }
}

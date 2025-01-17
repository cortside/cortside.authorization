using Microsoft.AspNetCore.Authorization;

namespace Cortside.Authorization.Client.AspNetCore {
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement> {
        private readonly IAuthorizationApiClient _client;

        public PermissionRequirementHandler(IAuthorizationApiClient client) {
            _client = client;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement) {
            if (await _client.HasPermissionAsync(context.User, requirement.PermissionName)) {
                context.Succeed(requirement);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Cortside.Authorization.Client.AspNetCore {
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider {

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options) {
        }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName) {
            var authorizationPolicy = await base.GetPolicyAsync(policyName);
            if (authorizationPolicy == null) {
                authorizationPolicy = new AuthorizationPolicyBuilder().AddRequirements(new PermissionRequirement(policyName)).Build();
            }

            return authorizationPolicy;
        }
    }
}

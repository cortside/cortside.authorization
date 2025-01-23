using Microsoft.AspNetCore.Authorization;

namespace Cortside.Authorization.Client.AspNetCore {
    public class PermissionRequirement : IAuthorizationRequirement {
        public PermissionRequirement(string permissionName) {
            PermissionName = permissionName;
        }

        public string PermissionName { get; private set; }
    }
}

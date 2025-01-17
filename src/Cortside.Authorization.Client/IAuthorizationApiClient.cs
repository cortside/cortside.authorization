using System.Security.Claims;

namespace Cortside.Authorization.Client {
    public interface IAuthorizationApiClient {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionName);
    }
}

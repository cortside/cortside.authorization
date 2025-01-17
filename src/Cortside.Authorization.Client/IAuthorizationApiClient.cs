using System.Security.Claims;
using Cortside.Authorization.Client.Models;
using Cortside.Authorization.Client.Models.Responses;

namespace Cortside.Authorization.Client {
    public interface IAuthorizationApiClient {
        Task<EvaluationResponse> EvaluatePolicyAsync(EvaluationDto dto);
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permissionName);
    }
}

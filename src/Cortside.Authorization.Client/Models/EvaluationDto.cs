using System.Security.Claims;

namespace Cortside.Authorization.Client.Models {
    public class EvaluationDto {
        public ClaimsPrincipal User { get; set; }
    }
}

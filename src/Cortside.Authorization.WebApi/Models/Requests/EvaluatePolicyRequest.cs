using System.Collections.Generic;

namespace Cortside.Authorization.WebApi.Models.Requests {
    /// <summary>
    /// Request body for policy evaluation
    /// </summary>
    public class EvaluatePolicyRequest {
        /// <summary>
        /// List of user's claims
        /// </summary>
        public List<ClaimModel> Claims { get; set; } = [];
    }
}

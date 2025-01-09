using System.Collections.Generic;

namespace Cortside.Authorization.WebApi.Models.Responses {
    /// <summary>
    /// Authorization model
    /// </summary>
    public class AuthorizationModel {
        /// <summary>
        /// Roles
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// Permissions
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}

using System.Collections.Generic;

namespace Cortside.Authorization.Dto {
    public class AuthorizationDto {
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}

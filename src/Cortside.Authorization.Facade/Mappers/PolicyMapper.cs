using System.Collections.Generic;
using System.Linq;
using Cortside.Authorization.Domain.Entities;
using Cortside.Authorization.Dto;

namespace Cortside.Authorization.Facade.Mappers {
    public class PolicyMapper {

        public PolicyMapper() {
        }


        internal AuthorizationDto MapToDto(IList<Role> roles) {
            var roleperms = roles.Select(r => r.RolePermissions);
            var perms = roleperms.SelectMany(rp => {

                var names = rp.Select(r => r.Permission.Name).Distinct();
                return names;
            });
            return new AuthorizationDto {
                Roles = roles?.Select(x => x.Name).ToList() ?? [],
                Permissions = roles?.SelectMany(x => x.RolePermissions.Select(p => p.Permission.Name)).Distinct().ToList() ?? [],
            };
        }
    }
}

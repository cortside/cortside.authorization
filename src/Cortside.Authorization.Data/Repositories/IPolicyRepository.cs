using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cortside.Authorization.Domain.Entities;

namespace Cortside.Authorization.Data.Repositories {
    public interface IPolicyRepository {
        Task<Policy> AddAsync(Policy policy);
        Task<IList<Role>> GetRolesByClaimsAsync(Guid policyResourceId, IEnumerable<KeyValuePair<string, string>> userClaims);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cortside.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Data.Repositories {
    public class PolicyRepository : IPolicyRepository {
        private readonly IDatabaseContext context;

        public PolicyRepository(IDatabaseContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }



        public async Task<Policy> AddAsync(Policy policy) {
            var entity = await context.Policies.AddAsync(policy);
            return entity.Entity;
        }

        public async Task<IList<Role>> GetRolesByClaimsAsync(Guid policyResourceId, IEnumerable<KeyValuePair<string, string>> userClaims) {
            // get the assigned role userClaims for the policy from the db
            var claimTypes = new List<string>(userClaims.Select(x => x.Key));
            var assigned = await context.PolicyRoleClaims
                .Include(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .Where(x => x.Role.Policy.PolicyResourceId == policyResourceId
                    && claimTypes.Contains(x.ClaimType))
                .ToListAsync();

            // get the ones that match to return
            var matched = assigned.Where(a => userClaims.Any(userClaim => userClaim.Key.Equals(a.ClaimType, StringComparison.OrdinalIgnoreCase) && userClaim.Value.Equals(a.Value, StringComparison.OrdinalIgnoreCase)));
            return matched.Select(x => x.Role).ToList();
        }
    }
}

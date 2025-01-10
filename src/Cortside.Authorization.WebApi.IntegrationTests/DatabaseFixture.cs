using System;
using System.Linq;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Authorization.Data;
using Cortside.Authorization.Domain.Entities;

namespace Cortside.Authorization.WebApi.IntegrationTests {
    public static class DatabaseFixture {
        public static void SeedInMemoryDb(DatabaseContext dbContext) {
            var subject = new Subject(Guid.Empty, string.Empty, string.Empty, string.Empty, "system");
            if (!dbContext.Subjects.Any(x => x.SubjectId == subject.SubjectId)) {
                dbContext.Subjects.Add(subject);
            }
            var policy = new Policy("Orders", "the policy for orders service");
            dbContext.Policies.Add(policy);
            var role = new Role("admin", "admin role", policy);
            dbContext.Roles.Add(role);
            var permission = new Permission("GetOrders", "can get orders", policy);
            dbContext.Permissions.Add(permission);
            var rolePermission = new RolePermission(role, permission);
            dbContext.RolePermissions.Add(rolePermission);
            var policyRoleClaim = new PolicyRoleClaim("sub", "132953b2-f6a7-4c1d-8da1-2b3c3dafe1c5", "this subject assigned to role", role); // from subjects.json
            dbContext.PolicyRoleClaims.Add(policyRoleClaim);


            // intentionally using this override to avoid the not implemented exception
            dbContext.SaveChanges(true);
        }
    }
}

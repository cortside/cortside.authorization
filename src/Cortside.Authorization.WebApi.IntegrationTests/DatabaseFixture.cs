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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var permission = new Permission("GetOrders", "can get orders");
            policy.AddPermission(permission);
            role.AddPermission(permission);
            var policyRoleClaim = new PolicyRoleClaim("sub", "132953b2-f6a7-4c1d-8da1-2b3c3dafe1c5", "this subject assigned to role"); // from subjects.json
            role.AddPolicyRoleClaim(policyRoleClaim);
            dbContext.Policies.Add(policy);


            // intentionally using this override to avoid the not implemented exception
            dbContext.SaveChanges(true);
        }
    }
}

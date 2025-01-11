using System;
using System.Linq;
using System.Threading.Tasks;
using Cortside.Authorization.Data;
using Cortside.Authorization.Data.Repositories;
using Cortside.Authorization.Domain.Entities;
using Cortside.Authorization.Dto;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Cortside.Authorization.DomainService.Tests {
    public class PolicyServiceTest : DomainServiceTest<PolicyService> {
        private readonly DatabaseContext db;
        private readonly Mock<ILogger<PolicyService>> loggerMock;

        public PolicyServiceTest() : base() {
            db = GetDatabaseContext();
            loggerMock = testFixture.Mock<ILogger<PolicyService>>();

            Service = new PolicyService(new PolicyRepository(db), loggerMock.Object);
        }

        [Fact]
        public async Task ShouldGetRolesByClaimsHappyPathAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            await db.Roles.AddAsync(role);
            var permission = new Permission("GetOrders", "can get orders", policy);
            await db.Permissions.AddAsync(permission);
            var rolePermission = new RolePermission(role, permission);
            await db.RolePermissions.AddAsync(rolePermission);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            await db.PolicyRoleClaims.AddAsync(policyRoleClaim);
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = policy.PolicyResourceId,
                Claims = [new ClaimDto { Type = policyRoleClaim.ClaimType, Value = policyRoleClaim.Value }]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNullOrEmpty().And.HaveCount(1);
            var actual = result.FirstOrDefault();
            actual.Name.Should().Be(role.Name);
            var actualPermissionsList = actual.RolePermissions.Select(x => x.Permission);
            actualPermissionsList.Should().NotBeNullOrEmpty().And.HaveCount(1);
            actualPermissionsList.Should().Contain(p => p.Name == permission.Name);
        }

        [Fact]
        public async Task ShouldNotGetRolesByClaimsInvalidPolicyIdAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            await db.Roles.AddAsync(role);
            var permission = new Permission("GetOrders", "can get orders", policy);
            await db.Permissions.AddAsync(permission);
            var rolePermission = new RolePermission(role, permission);
            await db.RolePermissions.AddAsync(rolePermission);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            await db.PolicyRoleClaims.AddAsync(policyRoleClaim);
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = Guid.NewGuid(),
                Claims = [new ClaimDto { Type = policyRoleClaim.ClaimType, Value = policyRoleClaim.Value }]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNull().And.HaveCount(0);
        }

        [Fact]
        public async Task ShouldGetRolesByClaimsWhenClaimAssignedToRoleAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            var unmatchedRole = new Role("unmatched", "", policy);
            await db.Roles.AddRangeAsync(role, unmatchedRole);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            await db.PolicyRoleClaims.AddAsync(policyRoleClaim);
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = policy.PolicyResourceId,
                Claims = [new ClaimDto { Type = policyRoleClaim.ClaimType, Value = policyRoleClaim.Value }]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNullOrEmpty().And.HaveCount(1);
            var actual = result.FirstOrDefault();
            actual.Name.Should().Be(role.Name);
        }

        [Fact]
        public async Task ShouldGetRolesByClaimsDistinctAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            await db.Roles.AddRangeAsync(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            var policyRoleClaim2 = new PolicyRoleClaim("groups", Guid.NewGuid().ToString(), "this group assigned to role", role);
            await db.PolicyRoleClaims.AddRangeAsync(policyRoleClaim, policyRoleClaim2); // same role, different claimtype/value
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = policy.PolicyResourceId,
                Claims = [
                    new ClaimDto { Type = policyRoleClaim.ClaimType, Value = policyRoleClaim.Value },
                    new ClaimDto { Type = policyRoleClaim2.ClaimType, Value = policyRoleClaim.Value },
                ]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNullOrEmpty().And.HaveCount(1, "two role claim matches, but for the same role");
            var actual = result.FirstOrDefault();
            actual.Name.Should().Be(role.Name);
        }

        [Fact]
        public async Task ShouldNotGetRolesByClaimsWhenNoValidClaimAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            await db.Roles.AddRangeAsync(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            await db.PolicyRoleClaims.AddAsync(policyRoleClaim);
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = policy.PolicyResourceId,
                Claims = [new ClaimDto { Type = "somethingelse", Value = "blah" }]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNull().And.HaveCount(0, "no valid claimtype && value");
        }

        [Fact]
        public async Task ShouldNotGetRolesByClaimsWhenNoValidClaimValueAsync() {
            // arrange
            var policy = new Policy("Orders", "the policy for orders service");
            await db.Policies.AddAsync(policy);
            var role = new Role("admin", "admin role", policy);
            await db.Roles.AddRangeAsync(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role", role);
            await db.PolicyRoleClaims.AddAsync(policyRoleClaim);
            await db.SaveChangesAsync();

            var dto = new EvaluatePolicyDto {
                PolicyResourceId = policy.PolicyResourceId,
                Claims = [new ClaimDto { Type = policyRoleClaim.ClaimType, Value = "something else" }]
            };

            // act
            var result = await Service.GetRolesByClaimsAsync(dto);

            // assert
            result.Should().NotBeNull().And.HaveCount(0, "valid claimtype, invalid value");
        }

    }
}

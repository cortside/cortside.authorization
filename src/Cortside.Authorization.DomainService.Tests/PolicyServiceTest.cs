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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var permission = new Permission("GetOrders", "can get orders");
            policy.AddPermission(permission);
            role.AddPermission(permission);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            await db.Policies.AddAsync(policy);
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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var permission = new Permission("GetOrders", "can get orders");
            policy.AddPermission(permission);
            role.AddPermission(permission);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            await db.Policies.AddAsync(policy);
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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var unmatchedRole = new Role("unmatched", "");
            policy.AddRole(unmatchedRole);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            await db.Policies.AddAsync(policy);
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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            var policyRoleClaim2 = new PolicyRoleClaim("groups", Guid.NewGuid().ToString(), "this group assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            role.AddPolicyRoleClaim(policyRoleClaim2); // same role, different claimtype/value
            await db.Policies.AddAsync(policy);
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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            await db.Policies.AddAsync(policy);
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
            var role = new Role("admin", "admin role");
            policy.AddRole(role);
            var policyRoleClaim = new PolicyRoleClaim("sub", Guid.NewGuid().ToString(), "this subject assigned to role");
            role.AddPolicyRoleClaim(policyRoleClaim);
            await db.Policies.AddAsync(policy);
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

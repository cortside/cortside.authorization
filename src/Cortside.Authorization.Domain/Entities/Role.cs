using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.Common.Messages.MessageExceptions;
using Cortside.Common.Validation;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {
    [Index(nameof(RoleResourceId), IsUnique = true)]
    [Table("Role")]
    [Comment("Roles within a policy")]
    public class Role : AuditableEntity {

        protected Role() { }

        /// <summary>
        /// Add a role to a policy by using the appropriate method on the policy
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Role(string name, string description) {
            RoleResourceId = Guid.NewGuid();
            Name = name;
            Description = description;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int RoleId { get; private set; }

        [Comment("Public unique identifier")]
        public Guid RoleResourceId { get; private set; } = Guid.NewGuid();

        [Column(TypeName = "nvarchar(100)")]
        [Comment("Name of the Role")]
        public string Name { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("Role description")]
        public string Description { get; private set; }

        [ForeignKey(nameof(PolicyId))]
        [Comment("FK to Policy")]
        public int PolicyId { get; private set; }
        public Policy Policy { get; private set; }

        private readonly List<RolePermission> rolePermissions = [];
        public virtual IReadOnlyList<RolePermission> RolePermissions => rolePermissions;
        public void AddPermission(Permission permission) {
            Guard.Against(() => permission.Policy != Policy, new BadRequestResponseException("The permission must share the same policy as the role, but does not"));
            var newRolePerm = new RolePermission(permission);
            rolePermissions.Add(newRolePerm);
        }

        private readonly List<PolicyRoleClaim> policyRoleClaims = [];
        public virtual IReadOnlyList<PolicyRoleClaim> PolicyRoleClaims => policyRoleClaims;
        public void AddPolicyRoleClaim(PolicyRoleClaim claim) {
            Guard.Against(() => policyRoleClaims.Any(c => c.ClaimType.Equals(claim.ClaimType, StringComparison.OrdinalIgnoreCase)
                    && c.Value.Equals(claim.Value, StringComparison.OrdinalIgnoreCase)),
                new BadRequestResponseException("Policy role claims' type and value must be unique within a role"));
            policyRoleClaims.Add(claim);
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {
    [Index(nameof(PolicyRoleClaimResourceId), IsUnique = true)]
    [Index(nameof(ClaimType), nameof(Value))]
    [Table("PolicyRoleClaim")]
    [Comment("PolicyRoleClaims within a role")]
    public class PolicyRoleClaim : AuditableEntity {

        protected PolicyRoleClaim() { }

        /// <summary>
        /// Add a claim to a role using the appropriate method on the role
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public PolicyRoleClaim(string claimType, string value, string description) {
            PolicyRoleClaimResourceId = Guid.NewGuid();
            ClaimType = claimType;
            Value = value;
            Description = description;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int PolicyRoleClaimId { get; private set; }

        [Comment("Public unique identifier")]
        public Guid PolicyRoleClaimResourceId { get; private set; } = Guid.NewGuid();

        [Column(TypeName = "nvarchar(100)")]
        [Comment("ClaimType of the PolicyRoleClaim, i.e. sub/role/group etc")]
        public string ClaimType { get; private set; }

        [Column(TypeName = "nvarchar(500)")]
        [Comment("Value of the claim")]
        public string Value { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("PolicyRoleClaim description")]
        public string Description { get; private set; }

        [ForeignKey(nameof(RoleId))]
        [Comment("FK to Role")]
        public int RoleId { get; private set; }
        public Role Role { get; private set; }
    }
}

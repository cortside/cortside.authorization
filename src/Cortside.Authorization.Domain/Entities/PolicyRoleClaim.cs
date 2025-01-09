using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("PolicyRoleClaim")]
    [Comment("PolicyRoleClaims within a role")]
    public class PolicyRoleClaim : AuditableEntity {

        protected PolicyRoleClaim() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int Id { get; private set; }

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

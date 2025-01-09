using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("Permission")]
    [Comment("Permissions within a role")]
#pragma warning disable CA1711
    public class Permission : AuditableEntity {
#pragma warning restore CA1711

        protected Permission() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int Id { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        [Comment("Name of the Permission")]
        public string Name { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("Permission description")]
        public string Description { get; private set; }

        [ForeignKey(nameof(RoleId))]
        [Comment("FK to Role")]
        public int RoleId { get; private set; }
        public Role Role { get; private set; }
    }
}

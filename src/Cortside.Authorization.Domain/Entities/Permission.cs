using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("Permission")]
    [Comment("Permissions available within a policy")]
#pragma warning disable CA1711
    public class Permission : AuditableEntity {
#pragma warning restore CA1711

        protected Permission() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int PermissionId { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        [Comment("Name of the Permission")]
        public string Name { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("Permission description")]
        public string Description { get; private set; }

        [ForeignKey(nameof(PolicyId))]
        [Comment("FK to Policy")]
        public int PolicyId { get; private set; }
        public Policy Policy { get; private set; }
    }
}

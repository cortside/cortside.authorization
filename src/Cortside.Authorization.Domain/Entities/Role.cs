using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("Role")]
    [Comment("Roles within a policy")]
    public class Role : AuditableEntity {

        protected Role() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int Id { get; private set; }

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

        private readonly List<Permission> permissions = [];
        public virtual IReadOnlyList<Permission> Permissions => permissions;
    }
}

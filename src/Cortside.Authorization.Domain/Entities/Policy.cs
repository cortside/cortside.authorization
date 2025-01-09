using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("Policy")]
    [Comment("Policies of the application")]
    public class Policy : AuditableEntity {

        protected Policy() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int Id { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        [Comment("Name of the policy")]
        public string Name { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("Policy description")]
        public string Description { get; private set; }

        private readonly List<Role> roles = [];
        public virtual IReadOnlyList<Role> Roles => roles;
    }
}

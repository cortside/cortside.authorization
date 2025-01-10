using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {
    [Index(nameof(RoleResourceId), IsUnique = true)]
    [Table("Role")]
    [Comment("Roles within a policy")]
    public class Role : AuditableEntity {

        protected Role() { }

        public Role(string name, string description, Policy policy) {
            RoleResourceId = Guid.NewGuid();
            Name = name;
            Description = description;
            Policy = policy;
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
    }
}

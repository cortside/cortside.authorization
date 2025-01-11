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
    [Index(nameof(PolicyResourceId), IsUnique = true)]
    [Index(nameof(Name), IsUnique = true)]
    [Table("Policy")]
    [Comment("Policies of the application")]
    public class Policy : AuditableEntity {

        protected Policy() { }

        public Policy(string name, string description) {
            PolicyResourceId = Guid.NewGuid();
            Name = name;
            Description = description;
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int PolicyId { get; private set; }

        [Comment("Public unique identifier")]
        public Guid PolicyResourceId { get; private set; } = Guid.NewGuid();

        [Column(TypeName = "nvarchar(100)")]
        [Comment("Name of the policy")]
        public string Name { get; private set; }

        [Column(TypeName = "nvarchar(255)")]
        [Comment("Policy description")]
        public string Description { get; private set; }

        private readonly List<Role> roles = [];
        public virtual IReadOnlyList<Role> Roles => roles;

        public void AddRole(Role role) {
            Guard.Against(() => roles.Any(r => r.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase)), new BadRequestResponseException("Role names must be unique within a policy"));
            roles.Add(role);
        }

        private readonly List<Permission> permissions = [];
        public virtual IReadOnlyList<Permission> Permissions => permissions;

        public void AddPermission(Permission permission) {
            Guard.Against(() => permissions.Any(r => r.Name.Equals(permission.Name, StringComparison.OrdinalIgnoreCase)), new BadRequestResponseException("Permission names must be unique within a policy"));
            permissions.Add(permission);
        }
    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cortside.AspNetCore.Auditable.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Domain.Entities {

    [Table("RolePermission")]
    [Comment("RolePermissions are permissions assigned to a role within a policy")]
#pragma warning disable CA1711
    public class RolePermission : AuditableEntity {
#pragma warning restore CA1711

        protected RolePermission() { }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("Auto incrementing id that is for internal use only")]
        public int RolePermissionId { get; private set; }


        [ForeignKey(nameof(RoleId))]
        [Comment("FK to Role")]
        public int RoleId { get; private set; }
        public Role Role { get; private set; }

        [ForeignKey(nameof(PermissionId))]
        [Comment("FK to Permission")]
        public int PermissionId { get; private set; }
        public Permission Permission { get; private set; }
    }
}

using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.EntityFramework;
using Cortside.Authorization.Domain.Entities;
using Cortside.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Data {
    public class DatabaseContext : UnitOfWorkContext<Subject>, IDatabaseContext {
        public DatabaseContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<Subject> subjectFactory) : base(options, subjectPrincipal, subjectFactory) {
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PolicyRoleClaim> PolicyRoleClaims { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("dbo");
            //modelBuilder.AddDomainEventOutbox();

            modelBuilder.SetDateTime();
            modelBuilder.SetCascadeDelete();
        }


    }
}

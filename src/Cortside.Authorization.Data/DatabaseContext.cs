using System;
using System.Linq;
using System.Threading.Tasks;
using Cortside.Authorization.Domain.Entities;
using Cortside.AspNetCore.Auditable;
using Cortside.AspNetCore.Auditable.Entities;
using Cortside.AspNetCore.EntityFramework;
using Cortside.Common.Security;
using Cortside.DomainEvent.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Cortside.Authorization.Data {
    public class DatabaseContext : UnitOfWorkContext<Subject>, IDatabaseContext {
        public DatabaseContext(DbContextOptions options, ISubjectPrincipal subjectPrincipal, ISubjectFactory<Subject> subjectFactory) : base(options, subjectPrincipal, subjectFactory) {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.AddDomainEventOutbox();

            // TOTO: make modelBuilder extensions
            SetDateTime(modelBuilder);
            SetCascadeDelete(modelBuilder);
        }

        /// <summary>
        /// Hook to add additional logic before entities are actually saved.  Shown here for example, only override if there is actual need.
        /// </summary>
        /// <param name="updatingSubject"></param>
        /// <returns></returns>
        protected override Task OnBeforeSaveChangesAsync(Subject updatingSubject) {
            return Console.Out.WriteLineAsync($"Change tracker has {ChangeTracker.Entries().Count()} entries");
        }
    }
}

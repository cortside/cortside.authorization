using System.Collections.Generic;
using Cortside.Authorization.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cortside.Authorization.Data {
    public interface IDatabaseContext {
        DbSet<Policy> Policies { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<PolicyRoleClaim> PolicyRoleClaims { get; set; }

        void RemoveRange(IEnumerable<object> entities);
        EntityEntry Remove(object entity);
    }
}

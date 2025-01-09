using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cortside.Authorization.Data {
    public interface IDatabaseContext {
        void RemoveRange(IEnumerable<object> entities);
        EntityEntry Remove(object entity);
    }
}

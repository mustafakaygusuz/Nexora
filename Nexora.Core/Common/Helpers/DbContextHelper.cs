using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Contexts;
using Nexora.Core.Data.EfCoreModels;

namespace Nexora.Core.Common.Helpers
{
    public static class DbContextHelper
    {
        public static void HandleSaveChanges(ChangeTracker changeTracker, ApiContext apiContext)
        {
            HandleAuditableEntities(changeTracker, apiContext);
            HandleAuditableStatefulEntities(changeTracker);
        }

        private static void HandleAuditableStatefulEntities(ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker
                            .Entries<AuditableStatefulBaseEntity>()
                            .Where(i => i.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.Entity.Status = StatusType.Deleted;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }

        private static void HandleAuditableEntities(ChangeTracker changeTracker, ApiContext apiContext)
        {
            foreach (var entry in changeTracker
                            .Entries<AuditableBaseEntity>()
                            .Where(i => i.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property<long>("CreatedBy").CurrentValue = (apiContext.UserId ?? apiContext.SystemUserId) ?? 0;
                        entry.Property<DateTime>("CreatedDate").CurrentValue = apiContext.CurrentDate;
                        break;
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        entry.Property<long?>("UpdatedBy").CurrentValue = apiContext.UserId ?? apiContext.SystemUserId;
                        entry.Property<DateTime?>("UpdatedDate").CurrentValue = apiContext.CurrentDate;
                        break;
                }
            }
        }
    }
}
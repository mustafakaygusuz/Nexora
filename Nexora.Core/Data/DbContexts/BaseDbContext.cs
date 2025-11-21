using Microsoft.EntityFrameworkCore;
using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Helpers;
using Nexora.Core.Contexts;
using Nexora.Core.Data.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Nexora.Core.Data.DbContexts
{
    public class BaseDbContext(DbContextOptions options, ApiContext apiContext) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Create the inequality expression: entity => entity.Status != "Deleted"

                if (typeof(IStateful).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "entity");
                    var filter = Expression.Lambda(Expression.NotEqual(Expression.Property(parameter, "Status"), Expression.Constant(StatusType.Deleted)), parameter);
                    entityType.SetQueryFilter(filter);
                }

                // Automatically convert to json objects in entities
                foreach (var property in entityType.ClrType.GetProperties())
                {
                    var jsonOwnedAttribute = property.GetCustomAttribute<SingularObjectJsonConversionAttribute>();
                    if (jsonOwnedAttribute != null)
                    {
                        modelBuilder.Entity(entityType.ClrType).OwnsOne(property.PropertyType, property.Name, onb =>
                        {
                            onb.ToJson();
                        });
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            DbContextHelper.HandleSaveChanges(ChangeTracker, apiContext);
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}

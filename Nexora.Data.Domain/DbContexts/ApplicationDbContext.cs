using Microsoft.EntityFrameworkCore;
using Nexora.Core.Contexts;
using Nexora.Core.Data.DbContexts;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.DbContexts
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ApiContext apiContext)
        : BaseDbContext(options, apiContext)
    {
        public virtual DbSet<Consumer> Consumers { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
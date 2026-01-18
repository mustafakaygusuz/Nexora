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
        public virtual DbSet<StaticText> StaticTexts { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<TranslationKey> TranslationKeys { get; set; }
        public virtual DbSet<TranslationValue> TranslationValues { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Model> Models { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
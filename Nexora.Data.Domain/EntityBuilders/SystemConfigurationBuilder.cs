using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class SystemConfigurationBuilder : IEntityTypeConfiguration<SystemConfiguration>
    {
        public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
        {
            builder.ToTable("SystemConfigurations");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Type).IsRequired();
            builder.Property(c => c.Key).HasMaxLength(500).IsRequired();
            builder.Property(c => c.Value);
        }
    }
}

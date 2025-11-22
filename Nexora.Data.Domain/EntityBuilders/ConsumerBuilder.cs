using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class ConsumerBuilder : IEntityTypeConfiguration<Consumer>
    {
        public void Configure(EntityTypeBuilder<Consumer> builder)
        {
            builder.ToTable("Consumers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Surname).HasMaxLength(100).IsRequired();
            builder.Property(c => c.Gender);
            builder.Property(c => c.BirthDate);
            builder.Property(e => e.AccessToken).HasMaxLength(2000);
            builder.Property(e => e.RefreshToken).HasMaxLength(2000);
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Status)
              .HasConversion<byte>()
              .HasColumnType("tinyint");
            // Also check Gender similarly if needed:
            builder.Property(c => c.Gender)
                   .HasConversion<byte?>()
                   .HasColumnType("tinyint");
            builder
                .HasIndex(c => new { c.Email, c.DeletedDate })
                .HasDatabaseName("IX_Consumer_Email_DeletedDate");
        }
    }
}
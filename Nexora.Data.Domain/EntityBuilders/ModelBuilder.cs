using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class ModelBuilder : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Models");

            builder.Property(m => m.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.Status)
                  .HasConversion<byte>()
                  .HasColumnType("tinyint");

            builder.Property(m => m.UpdatedDate);

            builder.HasOne(m => m.Brand)
                   .WithMany(b => b.Models)
                   .HasForeignKey(m => m.BrandId)
                   .HasConstraintName("fk_models_brands")
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class StaticTextBuilder : IEntityTypeConfiguration<StaticText>
    {
        public void Configure(EntityTypeBuilder<StaticText> builder)
        {
            builder.ToTable("StaticTexts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Type).IsRequired();
            builder.Property(c => c.Key).HasMaxLength(1000).IsRequired();

            builder.HasOne(b => b.ValueNavigation)
                   .WithMany(tk => tk.ValueNavigations)
                   .HasForeignKey(b => b.Value)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_StaticText_TranslationKey");
        }
    }
}
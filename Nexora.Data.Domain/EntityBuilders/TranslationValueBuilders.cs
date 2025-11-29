using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class TranslationValueBuilders : IEntityTypeConfiguration<TranslationValue>
    {
        public void Configure(EntityTypeBuilder<TranslationValue> builder)
        {
            builder.ToTable("TranslationValues");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.TranslationKeyId).IsRequired();
            builder.Property(c => c.Value).IsRequired();


            builder.Property(e => e.CreatedDate).HasColumnType("datetime");

            builder.Property(e => e.LanguageId)
                .IsUnicode(false)
                .IsFixedLength();

            builder.Property(e => e.UpdatedDate).HasColumnType("datetime");

            builder.HasOne(d => d.Language)
                .WithMany(p => p.TranslationValues)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TranslationValues_Languages");

            builder.HasOne(d => d.TranslationKey)
                .WithMany(p => p.TranslationValues)
                .HasForeignKey(d => d.TranslationKeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TranslationValues_TranslationKeys");
        }
    }
}

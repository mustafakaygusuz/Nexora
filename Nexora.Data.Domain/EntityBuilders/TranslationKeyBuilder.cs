using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexora.Data.Domain.Entities;

namespace Nexora.Data.Domain.EntityBuilders
{
    public class TranslationKeyBuilder : IEntityTypeConfiguration<TranslationKey>
    {
        public void Configure(EntityTypeBuilder<TranslationKey> builder)
        {
            builder.ToTable("TranslationKeys");

            builder.HasKey(tk => tk.Id);
            builder.Property(tk => tk.Id).ValueGeneratedOnAdd();
        }
    }
}
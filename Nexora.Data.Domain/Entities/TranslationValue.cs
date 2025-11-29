using Microsoft.EntityFrameworkCore;
using Nexora.Core.Data.EfCoreModels;

namespace Nexora.Data.Domain.Entities
{
    [Index(nameof(TranslationKeyId), nameof(LanguageId))]
    public class TranslationValue : AuditableBaseEntity
    {
        public long TranslationKeyId { get; set; }
        public string LanguageId { get; set; } = null!;
        public string? Value { get; set; }

        public virtual Language Language { get; set; } = null!;
        public virtual TranslationKey TranslationKey { get; set; } = null!;
    }
}
using Nexora.Core.Data.EfCoreModels;

namespace Nexora.Data.Domain.Entities
{
    public class TranslationKey : BaseEntity
    {
        public virtual ICollection<Language> Languages { get; set; } = null!;
        public virtual ICollection<TranslationValue> TranslationValues { get; set; } = [];
        public virtual ICollection<StaticText> ValueNavigations { get; set; } = null!;
    }
}
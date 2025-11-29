using Nexora.Core.Common.Enumerations;

namespace Nexora.Data.Domain.Entities
{
    public class Language
    {
        public string Id { get; set; } = null!;
        public string IsoCode { get; set; } = null!;
        public long Name { get; set; }
        public StatusType Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual TranslationKey NameNavigation { get; set; } = null!;
        public virtual ICollection<TranslationValue> TranslationValues { get; set; } = null!;
    }
}
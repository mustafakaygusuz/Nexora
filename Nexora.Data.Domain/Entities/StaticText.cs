using Nexora.Core.Data.EfCoreModels;
using Nexora.Data.Domain.Enumerations;

namespace Nexora.Data.Domain.Entities
{
    public class StaticText : AuditableStatefulBaseEntity
    {
        public long OrganizationId { get; set; }
        public string Key { get; set; } = null!;
        public long Value { get; set; }
        public StaticTextType Type { get; set; }

        //public TranslationKey ValueNavigation { get; set; } = null!;
    }
}
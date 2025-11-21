using Nexora.Core.Data.EfCoreModels;
using Nexora.Data.Domain.Enumerations;

namespace Nexora.Data.Domain.Entities
{
    public class SystemConfiguration : AuditableBaseEntity
    {
        public SystemConfigurationType Type { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
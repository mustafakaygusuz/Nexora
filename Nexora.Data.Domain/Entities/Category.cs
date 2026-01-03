using Nexora.Core.Data.EfCoreModels;
using Nexora.Data.Domain.Enumerations;

namespace Nexora.Data.Domain.Entities
{
    public class Category : StatefulBaseEntity
    {
        public required CategoryType Type { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
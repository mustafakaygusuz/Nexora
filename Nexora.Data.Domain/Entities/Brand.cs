using Nexora.Core.Data.EfCoreModels;

namespace Nexora.Data.Domain.Entities
{
    public class Brand : StatefulBaseEntity
    {
        public long CategoryId { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Category Category { get; set; } = null!;
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
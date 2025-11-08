using System.ComponentModel.DataAnnotations;

namespace Nexora.Core.Data.EfCoreModels
{
    public abstract class AuditableBaseEntity : BaseEntity
    {
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
    }
}
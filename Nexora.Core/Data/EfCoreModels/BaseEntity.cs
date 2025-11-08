using System.ComponentModel.DataAnnotations;

namespace Nexora.Core.Data.EfCoreModels
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
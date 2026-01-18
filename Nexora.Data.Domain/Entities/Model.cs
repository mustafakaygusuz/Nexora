using Nexora.Core.Data.EfCoreModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Data.Domain.Entities
{
    public class Model : StatefulBaseEntity
    {
        [Required]
        public long BrandId { get; set; }

        [MaxLength(200)]
        public required string Name { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;
    }
}

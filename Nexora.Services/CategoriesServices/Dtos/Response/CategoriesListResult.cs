using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Enumerations;
using Nexora.Data.Domain.Enumerations;

namespace Nexora.Services.CategoriesServices.Dtos.Response
{
    public class CategoriesListResult
    {
        [EncryptedId]
        public long Id { get; set; }
        public CategoryType Type { get; set; }
        public required string Name { get; set; }
        public StatusType Status { get; set; }
    }
}
using Nexora.Core.Common.Attributes;

namespace Nexora.Services.BrandsServices.Dtos.Response
{
    public class BrandsListByCategoryIdResult
    {
        [EncryptedId]
        public long Id { get; set; }
        public required string Name { get; set; }
    }
}
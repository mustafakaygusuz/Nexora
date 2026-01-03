using Nexora.Services.BrandsServices.Dtos.Response;

namespace Nexora.Services.BrandsServices
{
    public interface IBrandsService
    {
        /// <summary>
        /// List brands by category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<BrandsListByCategoryIdResult>> ListByCategoryId(long categoryId);
    }
}
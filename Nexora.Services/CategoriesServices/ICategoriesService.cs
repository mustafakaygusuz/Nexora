using Nexora.Services.CategoriesServices.Dtos.Response;

namespace Nexora.Services.CategoriesServices
{
    public interface ICategoriesService
    {
        /// <summary>
        /// List categories
        /// </summary>
        /// <returns></returns>
        Task<List<CategoriesListResult>> List();
    }
}
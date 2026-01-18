using Microsoft.AspNetCore.Mvc;
using Nexora.Services.CategoriesServices;
using Nexora.Services.CategoriesServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    public class CategoriesController(ICategoriesService _categoriesService) : BaseApiController
    {
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<CategoriesListResult>))]
        public async Task<IActionResult> List()
        {
            return OkResult(await _categoriesService.List());
        }
    }
}
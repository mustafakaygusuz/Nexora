using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;
using Nexora.Services.CategoriesServices;
using Nexora.Services.CategoriesServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoriesService _categoriesService) : ControllerBase
    {
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<CategoriesListResult>))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> List()
        {
            return Ok(await _categoriesService.List());
        }
    }
}
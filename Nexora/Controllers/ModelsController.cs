using Microsoft.AspNetCore.Mvc;
using Nexora.Services.ModelsServices;
using Nexora.Services.ModelsServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    public class ModelsController(IModelsService _modelsService) : BaseApiController
    {
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(List<ModelsListByBrandIdResult>))]
        public async Task<IActionResult> List([FromQuery] long brandId)
        {
            return OkResult(await _modelsService.ListByBrandId(brandId));
        }
    }
}

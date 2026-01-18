using Microsoft.AspNetCore.Mvc;
using Nexora.Services.BrandsServices;
using Nexora.Services.BrandsServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    public class BrandsController(IBrandsService _brandsService) : BaseApiController
    {
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(List<BrandsListByCategoryIdResult>))]
        public async Task<IActionResult> List([FromQuery] long categoryId)
        {
            return OkResult(await _brandsService.ListByCategoryId(categoryId));
        }
    }
}
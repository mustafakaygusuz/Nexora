using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;
using Nexora.Services.BrandsServices;
using Nexora.Services.BrandsServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BrandsController(IBrandsService _brandsService) : ControllerBase
    {
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(List<BrandsListByCategoryIdResult>))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> List([FromQuery] long categoryId)
        {
            return Ok(await _brandsService.ListByCategoryId(categoryId));
        }
    }
}
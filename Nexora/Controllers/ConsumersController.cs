using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;
using Nexora.Services.ConsumersServices;
using Nexora.Services.ConsumersServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConsumersController(IConsumersService _consumersService) : ControllerBase
    {
        [HttpPost("delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Delete()
        {
            await _consumersService.DeleteAccount();

            return NoContent();
        }

        [HttpGet("profile")]
        [ProducesResponseType(200, Type = typeof(ConsumersGetProfileResult))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _consumersService.GetProfile());
        }

        //[HttpPost("profile")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        //[ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        //[ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        //public async Task<IActionResult> Post(ConsumersUpdateProfileRequest request)
        //{
        //    await _consumersService.UpdateProfile(request);

        //    return NoContent();
        //}

        //[HttpGet("consents")]
        //[ProducesResponseType(200, Type = typeof(List<ConsumersListConsentsResult>))]
        //[ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        //[ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        //public async Task<IActionResult> GetConsents()
        //{
        //    return Ok(await _consumersService.GetConsents());
        //}

        //[HttpPost("consents")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        //[ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        //[ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        //public async Task<IActionResult> UpdateConsents(List<ConsumersUpdateConsentsRequest> request)
        //{
        //    await _consumersService.UpdateConsents(request);

        //    return NoContent();
        //}
    }
}

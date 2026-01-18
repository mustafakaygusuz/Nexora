using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;
using Nexora.Services.ConsumersServices;
using Nexora.Services.ConsumersServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    [Authorize]
    public class ConsumersController(IConsumersService _consumersService) : BaseApiController
    {
        [HttpPost("delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Delete()
        {
            await _consumersService.DeleteAccount();
            return NoContentResult();
        }

        [HttpGet("profile")]
        [ProducesResponseType(200, Type = typeof(ConsumersGetProfileResult))]
        public async Task<IActionResult> Get()
        {
            return OkResult(await _consumersService.GetProfile());
        }
    }
}

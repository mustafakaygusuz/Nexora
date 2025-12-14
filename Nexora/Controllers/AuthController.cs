using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Models;
using Nexora.Services.AuthServices;
using Nexora.Services.AuthServices.Dtos.Request;
using Nexora.Services.AuthServices.Dtos.Response;

namespace Nexora.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(201, Type = typeof(AuthRegisterResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(409, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Register(AuthRegisterRequest request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(AuthLoginResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(200, Type = typeof(AuthRefreshTokenResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _authService.RefreshToken(request));
        }

        [HttpGet("test")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(404, Type = typeof(ErrorResultModel))]
        [ProducesResponseType(500, Type = typeof(ErrorResultModel))]
        public async Task<IActionResult> Test()
        {
            return Ok("This service returned 200");
        }
    }
}
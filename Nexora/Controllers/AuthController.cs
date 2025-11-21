using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Nexora.Services.AuthServices;
using Nexora.Services.AuthServices.Dtos.Request;

namespace Nexora.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthRegisterRequest request)
        {
            return Ok(await _authService.Register(request));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthLoginRequest request)
        {
            return Ok(await _authService.Login(request));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            return Ok(await _authService.RefreshToken(request));
        }
    }
}
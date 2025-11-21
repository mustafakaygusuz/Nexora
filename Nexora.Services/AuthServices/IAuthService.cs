using Nexora.Services.AuthServices.Dtos.Request;
using Nexora.Services.AuthServices.Dtos.Response;

namespace Nexora.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthRegisterResponse> Register(AuthRegisterRequest request);
        Task<AuthLoginResponse> Login(AuthLoginRequest request);
        Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request);
    }
}
using Nexora.Data.ConsumersRepositories;
using Nexora.Services.AuthServices.Dtos.Request;
using Nexora.Services.AuthServices.Dtos.Response;

namespace Nexora.Services.AuthServices
{
    public class AuthService(IConsumersRepository _consumersRepository) : IAuthService
    {
        public async Task<AuthRegisterResponse> Register(AuthRegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthLoginResponse> Login(AuthLoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
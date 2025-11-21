using Nexora.Data.ConsumersRepositories;
using Nexora.Data.Domain.Entities;
using Nexora.Security.Hashing;
using Nexora.Security.JWT;
using Nexora.Services.AuthServices.Dtos.Request;
using Nexora.Services.AuthServices.Dtos.Response;

namespace Nexora.Services.AuthServices
{
    public class AuthService(IConsumersRepository _consumersRepository, ITokenHelper _tokenHelper) : IAuthService
    {
        public async Task<AuthRegisterResponse> Register(AuthRegisterRequest request)
        {
            var exists = await _consumersRepository.GetByEmailAsync(request.Email);
            if (exists != null)
                throw new Exception("Email already exists.");

            HashingHelper.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            var consumer = new Consumer
            {
                Nickname = request.Nickname,
                Email = request.Email,
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedDate = DateTime.UtcNow,
                Name = request.Name,
                Surname = request.Surname
            };
            var tokenResponse = _tokenHelper.CreateToken(consumer);

            consumer.AccessToken = tokenResponse.AccessToken;
            consumer.RefreshToken = tokenResponse.RefreshToken;

            consumer.Id = await _consumersRepository.InsertAsync(consumer);
            var response = new  AuthRegisterResponse
            {
                ExpireSeconds = tokenResponse.ExpireSeconds,
                RefreshExpireSeconds = tokenResponse.RefreshExpireSeconds,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken
            };  
            return response;
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
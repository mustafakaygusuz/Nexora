using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Data.ConsumersRepositories;
using Nexora.Data.Domain.Entities;
using Nexora.Security.Hashing;
using Nexora.Security.JWT;
using Nexora.Services.AuthServices.Dtos.Request;
using Nexora.Services.AuthServices.Dtos.Response;
using System.Net;

namespace Nexora.Services.AuthServices
{
    public class AuthService(IConsumersRepository _consumersRepository, ITokenHelper _tokenHelper) : IAuthService
    {
        public async Task<AuthRegisterResponse> Register(AuthRegisterRequest request)
        {
            var exists = await _consumersRepository.GetByEmailAsync(request.Email);
            if (exists != null)
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, StaticTextKeyType.AuthExEmlAlrdyExst);

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
                Surname = request.Surname,
                Status = StatusType.Active
            };
            var tokenResponse = _tokenHelper.CreateToken(consumer);

            consumer.AccessToken = tokenResponse.AccessToken;
            consumer.RefreshToken = tokenResponse.RefreshToken;

            consumer.Id = await _consumersRepository.InsertAsync(consumer);
            var response = new AuthRegisterResponse
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
            var consumer = await _consumersRepository.GetByEmailAsync(request.Email);
            if (consumer == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, StaticTextKeyType.AuthExUsrNtFnd);

            bool checkPassword = HashingHelper.VerifyPasswordHash(
                request.Password,
                consumer.PasswordHash,
                consumer.PasswordSalt
            );

            if (!checkPassword)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, StaticTextKeyType.AuthExInvldPsswrd);

            var tokenResponse = _tokenHelper.CreateToken(consumer);

            consumer.AccessToken = tokenResponse.AccessToken;
            consumer.RefreshToken = tokenResponse.RefreshToken;
            consumer.UpdatedDate = DateTime.UtcNow;

            _consumersRepository.UpdateTokens(
                consumer.Id,
                tokenResponse.AccessToken,
                tokenResponse.RefreshToken
            );

            return new AuthLoginResponse
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpireSeconds = tokenResponse.ExpireSeconds,
                RefreshExpireSeconds = tokenResponse.RefreshExpireSeconds
            };
        }


        public async Task<AuthRefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var consumer = await _consumersRepository.GetByRefreshTokenAsync(request.RefreshToken);
            if (consumer == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, StaticTextKeyType.AuthExInvldRfshTkn);

            var tokenResponse = _tokenHelper.CreateToken(consumer);

            consumer.AccessToken = tokenResponse.AccessToken;
            consumer.RefreshToken = tokenResponse.RefreshToken;
            consumer.UpdatedDate = DateTime.UtcNow;

            _consumersRepository.UpdateTokens(
                consumer.Id,
                consumer.AccessToken,
                consumer.RefreshToken
            );

            return new AuthRefreshTokenResponse
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpireSeconds = tokenResponse.ExpireSeconds,
                RefreshExpireSeconds = tokenResponse.RefreshExpireSeconds
            };
        }
    }
}
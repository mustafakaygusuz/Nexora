using Microsoft.IdentityModel.Tokens;
using Nexora.Core.Common.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Nexora.Core.Common.Helpers
{
    public sealed class JwtTokenHelper
    {
        public static (string AccessToken, string RefreshToken) GenerateToken(Dictionary<string, string>? claimDataDictionary, string accessTokenSecret, DateTime expireDate, string refreshTokenSecret = "", DateTime? refreshExpireDate = null)
        {
            if (claimDataDictionary != null)
            {
                claimDataDictionary.Add(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            }
            else
            {
                claimDataDictionary = new Dictionary<string, string> { { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() } };
            }

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = CreateTokenDescriptor(claimDataDictionary, accessTokenSecret, expireDate);

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            var accessToken = jwtSecurityTokenHandler.WriteToken(token);

            var refreshToken = refreshTokenSecret.HasValue() ? GenerateRefreshToken(claimDataDictionary, refreshTokenSecret, refreshExpireDate) : GenerateRefreshToken();

            return (accessToken, refreshToken);
        }

        public static bool AuthenticateToken(string token, string secretKey, out ClaimsPrincipal? claimsPrincipal)
        {
            claimsPrincipal = null;

            var principal = GetPrincipal(token, secretKey);

            if (principal?.Identity is not ClaimsIdentity claimsIdentity || !claimsIdentity.IsAuthenticated)
            {
                return false;
            }

            claimsPrincipal = principal;
            return true;
        }

        public static IEnumerable<Claim>? GetClaims(string token)
        {
            try
            {
                if (!(new JwtSecurityTokenHandler().ReadToken(token) is JwtSecurityToken { Claims: var claims }))
                {
                    return null;
                }

                return claims;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private static string GenerateRefreshToken(Dictionary<string, string>? claimDataDictionary, string refreshTokenSecret, DateTime? expireDate = null)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = CreateTokenDescriptor(claimDataDictionary, refreshTokenSecret, expireDate);

            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor CreateTokenDescriptor(Dictionary<string, string>? claimDataDictionary, string secret, DateTime? expireDate)
        {
            return new SecurityTokenDescriptor
            {
                Subject = claimDataDictionary.HasValue() ? new ClaimsIdentity(claimDataDictionary!.Select(x => new Claim(x.Key, x.Value))) : new ClaimsIdentity(),
                Expires = expireDate,
                NotBefore = DateTime.UtcNow.AddMinutes(-5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(secret)), SecurityAlgorithms.HmacSha256Signature)
            };
        }

        private static ClaimsPrincipal? GetPrincipal(string token, string secretKey)
        {
            try
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                if (jwtSecurityTokenHandler.ReadToken(token) is not JwtSecurityToken)
                {
                    return null;
                }

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true
                };

                return jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    }
}

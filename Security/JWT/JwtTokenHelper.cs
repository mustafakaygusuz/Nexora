using Microsoft.IdentityModel.Tokens;
using Nexora.Data.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Nexora.Security.JWT
{
    public class JwtTokenHelper : ITokenHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public JwtTokenHelper(string secretKey, string issuer, string audience,
            int accessTokenExpirationMinutes, int refreshTokenExpirationDays)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
            _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
            _refreshTokenExpirationDays = refreshTokenExpirationDays;
        }

        public TokenResponse CreateToken(Consumer consumer)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, consumer.Id.ToString()),
                new Claim(ClaimTypes.Email, consumer.Email),
                new Claim(ClaimTypes.Name, $"{consumer.Name} {consumer.Surname}"),
                new Claim("nickname", consumer.Nickname),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (consumer.Gender.HasValue)
                claims.Add(new Claim(ClaimTypes.Gender, consumer.Gender.Value.ToString()));

            if (consumer.BirthDate.HasValue)
                claims.Add(new Claim(ClaimTypes.DateOfBirth, consumer.BirthDate.Value.ToString("yyyy-MM-dd")));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: accessTokenExpiration,
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = CreateRefreshToken();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpireSeconds = _accessTokenExpirationMinutes * 60,
                RefreshExpireSeconds = _refreshTokenExpirationDays * 24 * 60 * 60
            };
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}

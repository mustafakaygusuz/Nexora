using Nexora.Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Security.JWT
{
    public interface ITokenHelper
    {
        TokenResponse CreateToken(Consumer consumer);
        ClaimsPrincipal? ValidateToken(string token);
        string CreateRefreshToken();
    }
}

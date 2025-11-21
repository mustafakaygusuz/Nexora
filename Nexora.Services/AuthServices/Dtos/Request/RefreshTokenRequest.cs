using System.ComponentModel.DataAnnotations;

namespace Nexora.Services.AuthServices.Dtos.Request
{
    public class RefreshTokenRequest
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
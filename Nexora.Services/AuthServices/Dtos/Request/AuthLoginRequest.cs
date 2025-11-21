using System.ComponentModel.DataAnnotations;

namespace Nexora.Services.AuthServices.Dtos.Request
{
    public class AuthLoginRequest
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
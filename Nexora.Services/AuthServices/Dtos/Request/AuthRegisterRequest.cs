using Nexora.Data.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Nexora.Services.AuthServices.Dtos.Request
{
    public class AuthRegisterRequest
    {
        [Required]
        public required string Nickname { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        [MaxLength(300)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(300)]
        public required string Surname { get; set; }
        public ConsumerGenderType? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Description { get; set; }
    }
}
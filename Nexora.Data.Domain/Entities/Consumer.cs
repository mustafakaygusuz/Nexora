using Nexora.Core.Data.EfCoreModels;
using Nexora.Data.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nexora.Data.Domain.Entities
{
    public class Consumer : StatefulBaseEntity
    {
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public required string Surname { get; set; }

        [MaxLength(100)]
        public required string Nickname { get; set; }

        [MaxLength(255)]
        public required string Email { get; set; }

        public ConsumerGenderType? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateOnly? BirthDate { get; set; }

        [MaxLength(500)]
        public required byte[] PasswordHash { get; set; }

        [MaxLength(500)]
        public required byte[] PasswordSalt { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public required DateTime CreatedDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime? DeletedDate { get; set; }
    }
}

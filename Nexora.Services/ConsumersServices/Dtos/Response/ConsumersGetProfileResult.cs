using Nexora.Data.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.ConsumersServices.Dtos.Response
{
    public sealed class ConsumersGetProfileResult
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public required string Email { get; set; }
        public ConsumerGenderType? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Description { get; set; }
    }
}
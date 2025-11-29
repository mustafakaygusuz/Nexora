using FluentValidation;
using Nexora.Core.Validation.Validators;
using Nexora.Services.AuthServices.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexora.Services.AuthServices.Dtos.Validators
{
    public class AuthRegisterRequestValidator : GenericValidator<AuthRegisterRequest>
    {
        public AuthRegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(255)
                .WithMessage("Email formatı geçersiz.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Şifre en az 1 büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az 1 küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az 1 sayı içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az 1 özel karakter içermelidir.");

            RuleFor(x => x.Nickname)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9._-]{3,30}$")
                .WithMessage("Nickname sadece harf, rakam ve ._- karakterleri içerebilir.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Surname)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.BirthDate)
                .Must(d => d == null || d <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Doğum tarihi gelecekte olamaz.");
        }
    }
}

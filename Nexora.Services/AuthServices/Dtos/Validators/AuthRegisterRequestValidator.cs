using FluentValidation;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Validation.Validators;
using Nexora.Services.AuthServices.Dtos.Request;

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
           .WithState(_ => StaticTextKeyType.AuthExInvalidEmail);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithState(_ => StaticTextKeyType.AuthExPwdReqUpper)
                .Matches("[a-z]").WithState(_ => StaticTextKeyType.AuthExPwdReqLower)
                .Matches("[0-9]").WithState(_ => StaticTextKeyType.AuthExPwdReqDigit)
                .Matches("[^a-zA-Z0-9]").WithState(_ => StaticTextKeyType.AuthExPwdReqSpecial);

            RuleFor(x => x.Nickname)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9._-]{3,30}$")
                .WithState(_ => StaticTextKeyType.AuthExInvalidNickname);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(300)
                .WithState(_ => StaticTextKeyType.AuthExInvalidName);

            RuleFor(x => x.Surname)
                .NotEmpty()
                .MaximumLength(300)
                .WithState(_ => StaticTextKeyType.AuthExInvalidSurname);

            RuleFor(x => x.BirthDate)
                .Must(d => d == null || d <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithState(_ => StaticTextKeyType.AuthExInvalidBirthDate);
        }
    }
}

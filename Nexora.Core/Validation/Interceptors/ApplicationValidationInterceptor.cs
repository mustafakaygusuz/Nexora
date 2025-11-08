using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Nexora.Core.Common.Enumerations;
using Nexora.Core.Common.Exceptions;
using Nexora.Core.Common.Extensions;
using System.Net;

namespace Nexora.Core.Validation.Interceptors
{
    public sealed class ApplicationValidationInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid && result.Errors.HasValue())
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, StaticTextKeyType.MdlVldtnExNtVld,
                    [.. result.Errors.Select(x => (StaticTextKeyType)x.CustomState)]);
            }

            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext) => commonContext;
    }
}
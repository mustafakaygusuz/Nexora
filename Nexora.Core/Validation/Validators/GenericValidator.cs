using FluentValidation;

namespace Nexora.Core.Validation.Validators
{
    /// <summary>
    /// Generic validator is a AbstractValidator<T> class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericValidator<T> : AbstractValidator<T> where T : class
    {
    }
}
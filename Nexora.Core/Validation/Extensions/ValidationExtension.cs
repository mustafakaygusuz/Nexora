using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Validation.Interceptors;
using System.Reflection;

namespace Nexora.Core.Validation.Extensions
{
    public static class ValidationExtension
    {
        /// <summary>
        /// Auto add validators from given assemblies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="internalTypes"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AutoAddValidators(this IServiceCollection services, Action<FluentValidationAutoValidationConfiguration>? options = null, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton, bool internalTypes = false, params Assembly[] assemblies)
        {
            if (!assemblies.HasValue())
            {
                return services;
            }

            var validationOption = new FluentValidationAutoValidationConfiguration();
            if (!options.HasValue())
            {
                validationOption.DisableDataAnnotationsValidation = true;
            }
            else
            {
                options?.Invoke(validationOption);
            }

            services.AddFluentValidationAutoValidation(options);

            services.AddValidatorsFromAssemblies(assemblies: assemblies, lifetime: serviceLifetime, includeInternalTypes: internalTypes);

            services.AddSingleton<IValidatorInterceptor, ApplicationValidationInterceptor>();

            return services;
        }

        /// <summary>
        /// Auto add validators from given assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="serviceLifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AutoAddValidators<T>(this IServiceCollection services, Action<FluentValidationAutoValidationConfiguration>? options = null, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            var validationOption = new FluentValidationAutoValidationConfiguration();
            if (!options.HasValue())
            {
                validationOption.DisableDataAnnotationsValidation = true;
            }
            else
            {
                options?.Invoke(validationOption);
            }

            services.AddFluentValidationAutoValidation(options);

            services.AddValidatorsFromAssemblyContaining<T>();

            services.AddSingleton<IValidatorInterceptor, ApplicationValidationInterceptor>();

            return services;
        }
    }
}
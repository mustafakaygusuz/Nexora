using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.Common.Attributes;
using Nexora.Core.Common.Configurations;
using Nexora.Core.Common.JsonConverters;
using System.Text.Json.Serialization;

namespace Nexora.Core.StartupExtensions
{
    public static class EncryptionExtension
    {
        public static IServiceCollection AddEncryption(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection("EncryptionConfiguration");

            if (configurationSection.GetChildren().Any())
            {
                services.Configure<EncryptionConfigurationModel>(configurationSection.Bind);

                services.AddControllers(opts =>
                {
                    opts.Filters.Add(new EncryptIdForCreatedResultAttribute(configurationSection.Get<EncryptionConfigurationModel>()!));
                })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opts.JsonSerializerOptions.Converters.Add(new EncryptIdConverterFactory(configurationSection.Get<EncryptionConfigurationModel>()!));
                });
            }

            return services;
        }
    }
}
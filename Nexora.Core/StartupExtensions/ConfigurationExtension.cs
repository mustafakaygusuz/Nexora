using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.Common.Configurations;

namespace Nexora.Core.StartupExtensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSmtpConfigurationModel>(options => configuration.GetSection("MailSmtpConfiguration").Bind(options));

            services.Configure<StackExchangeConfigurationModel>(options => configuration.GetSection("StackExchangeConfiguration").Bind(options));

            services.Configure<RateLimiterConfigurationModel>(options => configuration.GetSection("RateLimiterConfiguration").Bind(options));

            services.Configure<EncryptionConfigurationModel>(options => configuration.GetSection("EncryptionConfiguration").Bind(options));

            services.Configure<CacheLockConfigurationModel>(options => configuration.GetSection("CacheLockConfiguration").Bind(options));

            services.Configure<SwaggerConfigurationModel>(options => configuration.GetSection("SwaggerConfiguration").Bind(options));

            return services;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.Caching.Services;
using Nexora.Core.Common.Configurations;
using StackExchange.Redis;

namespace Nexora.Core.StartupExtensions
{
    public static class CacheExtension
    {
        public static IServiceCollection AddCacheWithStackExchange(this IServiceCollection services, IConfiguration configuration)
        {
            var stackExchangeConfiguration = configuration.GetSection("StackExchangeConfiguration").Get<StackExchangeConfigurationModel>()!;

            services.AddScoped<ICacheLockService, CacheLockService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                AsyncTimeout = stackExchangeConfiguration.AsyncTimeout,
                SyncTimeout = stackExchangeConfiguration.SyncTimeout,
                ConnectTimeout = stackExchangeConfiguration.ConnectTimeout,
                EndPoints = { { stackExchangeConfiguration.Host, stackExchangeConfiguration.Port } },
                Password = stackExchangeConfiguration.Password,
                AbortOnConnectFail = stackExchangeConfiguration.AbortOnConnectFail,
                Ssl = stackExchangeConfiguration.Ssl
            }));

            return services;
        }
    }
}
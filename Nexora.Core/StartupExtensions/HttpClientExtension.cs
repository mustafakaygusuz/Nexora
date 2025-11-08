using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.HttpClientServices;

namespace Nexora.Core.StartupExtensions
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddHttpClientWithSSL(this IServiceCollection services, TimeSpan? timeout = null)
        {
            services.AddHttpClient("SSL", options =>
            {
                options.Timeout = timeout ?? TimeSpan.FromSeconds(10);
            });

            services.AddSingleton<IHttpClientService, HttpClientService>();

            return services;
        }

        public static IServiceCollection AddHttpClientWithoutSSL(this IServiceCollection services, TimeSpan? timeout = null)
        {
            services.AddHttpClient("NoSSL", options =>
            {
                options.Timeout = timeout ?? TimeSpan.FromSeconds(10);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, certChain, policyErrors) => true
                };
            });

            services.AddSingleton<IHttpClientService, HttpClientService>();

            return services;
        }
    }
}
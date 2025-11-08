using Microsoft.Extensions.DependencyInjection;
using Nexora.Core.Data.DapperRepository;

namespace Nexora.Core.StartupExtensions
{
    public static class DapperExtension
    {
        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            services.AddSingleton<IDapperRepository, DapperRepository>();

            return services;
        }
    }
}
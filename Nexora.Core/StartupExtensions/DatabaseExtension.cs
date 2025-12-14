using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nexora.Core.StartupExtensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabase<TDbContext>(this IServiceCollection services, IConfiguration configuration, bool enableSensitiveDataLogging = false)
            where TDbContext : DbContext
        {
            services
                .AddDbContext<TDbContext>(options =>
                {
                    options
                        .UseNpgsql(configuration.GetConnectionString(typeof(TDbContext).Name), options =>
                        {
                            options.EnableRetryOnFailure();
                            options.MigrationsAssembly(typeof(TDbContext).Assembly.GetName().Name);
                        });

                    options.ConfigureWarnings(builder =>
                    {
                        builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                    });

                    if (enableSensitiveDataLogging)
                    {
                        options.EnableSensitiveDataLogging();
                    }
                });

            //services.MigrateDatabase<TDbContext>();

            return services;
        }

        private static void MigrateDatabase<TDbContext>(this IServiceCollection serviceCollection)
            where TDbContext : DbContext
        {
            var copiedServiceCollection = new ServiceCollection();

            foreach (var service in serviceCollection)
            {
                copiedServiceCollection.Insert(0, service);
            }

            using var serviceScope = copiedServiceCollection.BuildServiceProvider().GetService<IServiceScopeFactory>()?.CreateScope();

            if (serviceScope != null)
            {
                var services = serviceScope.ServiceProvider;

                var databaseContext = services.GetRequiredService<TDbContext>();

                databaseContext.Database.Migrate();
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Nexora.Core.Caching.Constants;
using Nexora.Core.Caching.Services;
using Nexora.Core.Common.Extensions;
using Nexora.Core.Contexts;
using Nexora.Data.Domain.DbContexts;
using Nexora.Data.Domain.Enumerations;

namespace Nexora.Data.SystemConfigurationManagers
{
    public sealed class SystemConfigurationManager(
        ApplicationDbContext _dbContext,
        ICacheService _cacheService,
        ApiContext _apiContext) : ISystemConfigurationManager
    {
        /// <summary>
        /// Get organization system configurations by type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<T> GetByType<T>(SystemConfigurationType type) where T : class, new()
        {
            var entity = await _cacheService.GetAndSet(CacheKeys.SystemConfigurationsList(
                type.ToString()), async () =>
                await _dbContext.SystemConfigurations
                    .AsNoTracking()
                    .Where(x =>
                        x.Type == type)
                    .ToDictionaryAsync(x => x.Key, x => x.Value),
                    _apiContext.CurrentDate.AddDays(1));

            if (entity.HasValue())
            {
                return entity.ToObject<T>();
            }

            return new();
        }
    }
}

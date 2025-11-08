using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Caching.Services
{
    public sealed class CacheLockService(
        //ICacheService _cacheService,
        //IOptions<CacheLockConfigurationModel> _cacheLockConfiguration,
        //ApiContext _apiContext
        ) : ICacheLockService
    {
        /// <summary>
        /// Cooldown lock for rate limiting
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        //public async Task<int> CooldownLock(CacheLockType lockType, params string[] keys)
        //{
        //    var cacheLockConfiguration = _cacheLockConfiguration.Value?.CacheLockConfigurations?.FirstOrDefault(x => x.Name == lockType.ToString());

        //    if (cacheLockConfiguration.HasValue())
        //    {
        //        return await _cacheService.ScriptEvaluate(CachingScripts.CooldownRateLimiterScript,
        //        new
        //        {
        //            key = CacheKeys.CacheLockCounter(lockType, $"{_apiContext.OrganizationId}_{string.Join("_", keys)}"),
        //            expiry = cacheLockConfiguration!.CounterSeconds,
        //            maxRequests = cacheLockConfiguration.RepeatLockCount,
        //            limitKey = CacheKeys.CacheLockLimit(lockType, $"{_apiContext.OrganizationId}_{string.Join("_", keys)}"),
        //            limitExpire = cacheLockConfiguration.LockSeconds
        //        }) ?? default;
        //    }

        //    return default;
        //}

        /// <summary>
        /// Fixed window lock for rate limiting
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        //public async Task<int> FixedWindowLock(CacheLockType lockType, params string[] keys)
        //{
        //    var cacheLockConfiguration = _cacheLockConfiguration.Value?.CacheLockConfigurations?.FirstOrDefault(x => x.Name == lockType.ToString());

        //    if (cacheLockConfiguration.HasValue())
        //    {
        //        return await _cacheService.ScriptEvaluate(CachingScripts.FixedWindowRateLimiterScript,
        //        new
        //        {
        //            key = CacheKeys.CacheLockLimit(lockType, $"{_apiContext.OrganizationId}_{string.Join("_", keys)}"),
        //            expiry = cacheLockConfiguration!.LockSeconds,
        //            maxRequests = cacheLockConfiguration.RepeatLockCount
        //        }) ?? default;
        //    }

        //    return default;
        //}
        public Task<int> CooldownLock(CacheLockType lockType, params string[] keys)
        {
            throw new NotImplementedException();
        }

        public Task<int> FixedWindowLock(CacheLockType lockType, params string[] keys)
        {
            throw new NotImplementedException();
        }
    }
}
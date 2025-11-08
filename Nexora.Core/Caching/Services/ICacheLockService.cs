using Nexora.Core.Common.Enumerations;

namespace Nexora.Core.Caching.Services
{
    public interface ICacheLockService
    {
        /// <summary>
        /// Cooldown lock for rate limiting
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        Task<int> CooldownLock(CacheLockType lockType, params string[] keys);

        /// <summary>
        /// Fixed window lock for rate limiting
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        Task<int> FixedWindowLock(CacheLockType lockType, params string[] keys);
    }
}
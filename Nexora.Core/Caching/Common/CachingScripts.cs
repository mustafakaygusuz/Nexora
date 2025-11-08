using StackExchange.Redis;

namespace Nexora.Core.Caching.Common
{
    public static class CachingScripts
    {
        public static LuaScript FixedWindowRateLimiterScript => LuaScript.Prepare(FixedWindowRateLimiter);
        public static LuaScript CooldownRateLimiterScript => LuaScript.Prepare(CooldownRateLimiter);
        public static LuaScript SetKeyExpireIfLessScript => LuaScript.Prepare(SetKeyExpireIfLess);

        private const string FixedWindowRateLimiter = @"
            local count = redis.call('INCR',@key)
            if count == 1 then
                redis.call('EXPIRE', @key, @expiry)
            end
            if count < tonumber(@maxRequests) then
                return 0
            else
                return 1
            end
            ";

        private const string CooldownRateLimiter = @"
            local limitExist = redis.call('EXISTS', @limitKey)
            if limitExist == 1 then
                return 1
            else
                local count = redis.call('INCR',@key)
                if count == 1 then
                    redis.call('EXPIRE', @key, @expiry)
                end
                if count < tonumber(@maxRequests) then
                    return 0
                else
                    redis.call('SET', @limitKey, 1, 'EX', @limitExpire)
                    return 1
                end
            end
            ";

        private const string SetKeyExpireIfLess = @"
            local currentTTL = redis.call('TTL', @key)
            if currentTTL == -2 then
                return -2  -- No Key
            end
            if currentTTL == -1 or tonumber(@expiry) < currentTTL then
                redis.call('EXPIRE', @key, @expiry)
                return tonumber(@expiry)
            end
            return currentTTL
            ";
    }
}
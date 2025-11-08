using Microsoft.Extensions.Logging;
using Nexora.Core.Common.Extensions;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace Nexora.Core.Caching.Services
{
    public sealed class CacheService(IConnectionMultiplexer _connectionMultiplexer, ILogger<CacheService> _logger) : ICacheService
    {
        private IDatabase GetRedisClient() => _connectionMultiplexer.GetDatabase();

        public async Task<T?> Get<T>(string key, bool compress = false)
        {
            try
            {
                if (compress)
                {
                    var cacheCompressedData = await GetRedisClient().StringGetAsync(key, CommandFlags.PreferReplica);

                    if (cacheCompressedData.HasValue())
                    {
                        return CompressionExtensions.DecompressFromBytes<T>(cacheCompressedData.ToConvert<byte[]>()!);
                    }

                    return default;
                }

                var value = await GetRedisClient().StringGetAsync(key);

                return value.ToString().FromJson<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<T> Get<T>(string key, Func<Task<T>> dataSelector, bool compress = false)
        {
            try
            {
                var value = await Get<T>(key, compress);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await dataSelector();
        }

        public async Task<T> GetAndSet<T>(string key, Func<Task<T>> dataSelector, DateTime? expireDate = null, bool compress = false)
        {
            try
            {
                var value = await Get<T>(key, compress);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            var data = await dataSelector();

            if (data.HasValue())
            {
                await Set(key, data, expireDate, compress);
            }

            return data;
        }

        public async Task<T?> GetHash<T>(string hashId, string key, bool compress = false)
        {
            try
            {
                if (compress)
                {
                    var cacheCompressedData = await GetRedisClient().HashGetAsync(hashId, key, CommandFlags.PreferReplica);

                    if (cacheCompressedData.HasValue())
                    {
                        return CompressionExtensions.DecompressFromBytes<T>(cacheCompressedData.ToConvert<byte[]>()!);
                    }

                    return default;
                }

                var value = await GetRedisClient().HashGetAsync(hashId, key, CommandFlags.PreferReplica);

                return value.ToString().FromJson<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<T> GetHash<T>(string hashId, string key, Func<Task<T>> dataSelector, bool compress = false)
        {
            try
            {
                var value = await GetHash<T>(hashId, key, compress);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await dataSelector();
        }

        public async Task<T> GetAndSetHash<T>(string hashId, string key, Func<Task<T>> dataSelector, int? expireSecond = null, bool compress = false)
        {
            try
            {
                var value = await GetHash<T>(hashId, key, compress);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            var data = await dataSelector();

            if (data.HasValue())
            {
                await SetHash(hashId, key, data, expireSecond, compress);
            }

            return data;
        }

        public async Task<T?> GetJson<T>(string key)
        {
            try
            {
                var result = await GetRedisClient().ExecuteAsync("JSON.GET", key);

                return result.ToString().FromJson<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<T> GetJson<T>(string key, Func<Task<T>> dataSelector)
        {
            try
            {
                var value = await GetJson<T>(key);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await dataSelector();
        }

        public async Task<T> GetAndSetJson<T>(string key, Func<Task<T>> dataSelector)
        {
            try
            {
                var value = await GetJson<T>(key);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            var data = await dataSelector();

            if (data.HasValue())
            {
                await SetJson(key, data);
            }

            return data;
        }

        public async Task<T?> GetJson<T>(string key, Expression<Func<T, bool>> query)
        {
            try
            {
                var result = await GetRedisClient().ExecuteAsync("JSON.GET", key, query.CreateJsonPathQuery());

                return result.ToString().FromJson<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<T> GetJson<T>(string key, Expression<Func<T, bool>> query, Func<Task<T>> dataSelector)
        {
            try
            {
                var value = await GetJson<T>(key, query);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await dataSelector();
        }

        public async Task<T> GetAndSetJson<T>(string key, Expression<Func<T, bool>> query, Func<Task<T>> dataSelector)
        {
            try
            {
                var value = await GetJson<T>(key, query);

                if (value != null)
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            var data = await dataSelector();

            if (data.HasValue())
            {
                await SetJson(key, data);
            }

            return data;
        }

        public async Task<bool> CheckKeyExist(string key)
        {
            try
            {
                return await GetRedisClient().KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<long?> Increment(string key, uint value)
        {
            try
            {
                return await GetRedisClient().StringIncrementAsync(key, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<long?> IncrementHash(string hashId, string key, uint value)
        {
            try
            {
                return await GetRedisClient().HashIncrementAsync(hashId, key, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<bool> IncrementHash(Dictionary<string, string> keys, uint value)
        {
            try
            {
                var redisClient = GetRedisClient();

                var transaction = redisClient.CreateTransaction();

                foreach (var hashKey in keys)
                {
                    _ = transaction.HashIncrementAsync(hashKey.Key, hashKey.Value, value);
                }

                return await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> IncrementHash(Dictionary<string, List<string>> keys, uint value)
        {
            try
            {
                var redisClient = GetRedisClient();
                var increased = new Dictionary<string, List<string>>();

                foreach (var hashKey in keys)
                {
                    foreach (var key in hashKey.Value)
                    {
                        if (await redisClient.HashExistsAsync(hashKey.Key, key))
                        {
                            if (increased.TryGetValue(hashKey.Key, out var increasedList))
                            {
                                increasedList.Add(key);
                            }
                            else
                            {
                                increased.Add(hashKey.Key, [key]);
                            }
                        }
                    }
                }

                if (increased.HasValue())
                {
                    var transaction = redisClient.CreateTransaction();

                    foreach (var hashKey in increased)
                    {
                        foreach (var key in hashKey.Value)
                        {
                            _ = transaction.HashIncrementAsync(hashKey.Key, key, value);
                        }
                    }

                    await transaction.ExecuteAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<long?> Decrement(string key, uint value)
        {
            try
            {
                return await GetRedisClient().StringDecrementAsync(key, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<long?> DecrementHash(string hashId, string key, uint value)
        {
            try
            {
                return await GetRedisClient().HashDecrementAsync(hashId, key, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<bool> DecrementHash(Dictionary<string, string> keys, uint value)
        {
            try
            {
                var redisClient = GetRedisClient();

                var transaction = redisClient.CreateTransaction();

                foreach (var hashKey in keys)
                {
                    _ = transaction.HashDecrementAsync(hashKey.Key, hashKey.Value, value);
                }

                return await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> DecrementHash(Dictionary<string, List<string>> keys, uint value)
        {
            try
            {
                var redisClient = GetRedisClient();
                var decreased = new Dictionary<string, List<string>>();

                foreach (var hashKey in keys)
                {
                    foreach (var key in hashKey.Value)
                    {
                        if (await redisClient.HashExistsAsync(hashKey.Key, key))
                        {
                            if (decreased.TryGetValue(hashKey.Key, out var decreasedList))
                            {
                                decreasedList.Add(key);
                            }
                            else
                            {
                                decreased.Add(hashKey.Key, [key]);
                            }
                        }
                    }
                }

                if (decreased.HasValue())
                {
                    var transaction = redisClient.CreateTransaction();

                    foreach (var hashKey in decreased)
                    {
                        foreach (var key in hashKey.Value)
                        {
                            _ = transaction.HashDecrementAsync(hashKey.Key, key, value);
                        }
                    }

                    await transaction.ExecuteAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<IAsyncEnumerable<KeyValuePair<string, string>>?> ScanHash(string hashId, string pattern, int pageSize = 1000)
        {
            try
            {
                return await Task.FromResult(ScanHashInternal(hashId, pattern, pageSize));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public async Task<bool> Set<T>(string key, T value, bool compress = false)
        {
            try
            {
                if (compress)
                {
                    var cacheCompressedData = CompressionExtensions.CompressAsBytes(value);
                    return await GetRedisClient().StringSetAsync(key, cacheCompressedData);
                }
                else
                {
                    return await GetRedisClient().StringSetAsync(key, value.ToJson(false));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> Set<T>(string key, T value, DateTime expireDate, bool compress = false)
        {
            try
            {
                if (compress)
                {
                    var cacheCompressedData = CompressionExtensions.CompressAsBytes(value);
                    return await GetRedisClient().StringSetAsync(key, cacheCompressedData, expireDate - DateTime.Now);
                }
                else
                {
                    return await GetRedisClient().StringSetAsync(key, value.ToJson(false), expireDate - DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> Set<T>(string key, T value, TimeSpan expireTime, bool compress = false)
        {
            try
            {
                if (compress)
                {
                    var cacheCompressedData = CompressionExtensions.CompressAsBytes(value);
                    return await GetRedisClient().StringSetAsync(key, cacheCompressedData, expireTime);
                }
                else
                {
                    return await GetRedisClient().StringSetAsync(key, value.ToJson(false), expireTime);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetHash<T>(string hashId, string key, T value, bool compress = false)
        {
            try
            {
                var hashData = value.ToJson(false);

                if (compress)
                {
                    hashData = CompressionExtensions.CompressAsString(hashData);
                }

                return await GetRedisClient().HashSetAsync(hashId, key, hashData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetHash<T>(string hashId, string key, T value, int expireSecond, bool compress = false)
        {
            try
            {
                var redisClient = GetRedisClient();
                var hashData = value.ToJson(false);

                if (compress)
                {
                    hashData = CompressionExtensions.CompressAsString(hashData);
                }

                if (!await redisClient.HashExistsAsync(hashId, key))
                {
                    var transaction = redisClient.CreateTransaction();
                    _ = transaction.HashSetAsync(hashId, key, hashData);
                    _ = transaction.ExecuteAsync("EXPIRE", hashId, expireSecond);
                    return await transaction.ExecuteAsync();
                }
                else
                {
                    return await redisClient.HashSetAsync(hashId, key, hashData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetJson<T>(string key, T value)
        {
            try
            {
                var result = await GetRedisClient().ExecuteAsync("JSON.SET", key, ".", value.ToJson(false));

                return result.HasValue() && result.ToString() == "OK";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetKeyExpire(string key, DateTime expireDate)
        {
            try
            {
                return await GetRedisClient().KeyExpireAsync(key, expireDate - DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetKeyExpire(string key, TimeSpan expireTime)
        {
            try
            {
                return await GetRedisClient().KeyExpireAsync(key, expireTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetIfNotExists<T>(string key, T value)
        {
            try
            {
                return await GetRedisClient().StringSetAsync(key, value.ToJson(false), when: When.NotExists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> SetIfNotExists<T>(string key, T value, int expireSecond)
        {
            try
            {
                return await GetRedisClient().StringSetAsync(key, value.ToJson(false), TimeSpan.FromSeconds(expireSecond), When.NotExists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> Remove(string key)
        {
            try
            {
                return await GetRedisClient().KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> RemoveByPattern(string pattern)
        {
            try
            {
                await foreach (var key in GetKeysAsync(pattern))
                {
                    await GetRedisClient().KeyDeleteAsync(key);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> RemoveByPattern(string hashId, string pattern)
        {
            try
            {
                var redisClient = GetRedisClient();

                var scanResult = redisClient.HashScanAsync(hashId, pattern);

                var transaction = redisClient.CreateTransaction();

                await foreach (var key in scanResult)
                {
                    _ = transaction.HashDeleteAsync(hashId, key.Name);
                }

                return await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> RemoveFromHash(string hashId, string key)
        {
            try
            {
                return await GetRedisClient().HashDeleteAsync(hashId, key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<bool> RemoveFromHash(string hashId, List<string> keys)
        {
            try
            {
                var transaction = GetRedisClient().CreateTransaction();

                foreach (var key in keys)
                {
                    _ = transaction.HashDeleteAsync(hashId, key);
                }

                return await transaction.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        public async Task<int?> ScriptEvaluate(LuaScript script, object? parameters = null)
        {
            try
            {
                var result = await GetRedisClient().ScriptEvaluateAsync(script, parameters);

                return result.IsNull ? null : (int?)result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        private async Task Set<T>(string key, T value, DateTime? expireDate, bool compress = false)
        {
            try
            {
                if (expireDate == null)
                {
                    await Set(key, value, compress);
                }
                else
                {
                    await Set(key, value, expireDate.Value, compress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task SetHash<T>(string hashId, string key, T value, int? expireSecond, bool compress = false)
        {
            try
            {
                if (expireSecond == null)
                {
                    await SetHash(hashId, key, value, compress);
                }
                else
                {
                    await SetHash(hashId, key, value, expireSecond.Value, compress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async IAsyncEnumerable<KeyValuePair<string, string>> ScanHashInternal(string hashId, string pattern, int pageSize = 1000)
        {
            var database = GetRedisClient();

            await foreach (var a in database.HashScanAsync(hashId, pattern, pageSize))
            {
                yield return new KeyValuePair<string, string>(a.Name!, a.Value!);
            }
        }

        private async IAsyncEnumerable<string> GetKeysAsync(string pattern)
        {
            foreach (var endpoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endpoint);
                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }
        }
    }
}
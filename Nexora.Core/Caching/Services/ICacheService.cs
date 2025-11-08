using StackExchange.Redis;
using System.Linq.Expressions;

namespace Nexora.Core.Caching.Services
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string key, bool compress = false);

        Task<T> Get<T>(string key, Func<Task<T>> dataSelector, bool compress = false);

        Task<T> GetAndSet<T>(string key, Func<Task<T>> dataSelector, DateTime? expireDate = null, bool compress = false);

        Task<T?> GetHash<T>(string hashId, string key, bool compress = false);

        Task<T> GetHash<T>(string hashId, string key, Func<Task<T>> dataSelector, bool compress = false);

        Task<T> GetAndSetHash<T>(string hashId, string key, Func<Task<T>> dataSelector, int? expireSecond = null, bool compress = false);

        Task<T?> GetJson<T>(string key);

        Task<T> GetJson<T>(string key, Func<Task<T>> dataSelector);

        Task<T> GetAndSetJson<T>(string key, Func<Task<T>> dataSelector);

        Task<T?> GetJson<T>(string key, Expression<Func<T, bool>> query);

        Task<T> GetJson<T>(string key, Expression<Func<T, bool>> query, Func<Task<T>> dataSelector);

        Task<T> GetAndSetJson<T>(string key, Expression<Func<T, bool>> query, Func<Task<T>> dataSelector);

        Task<bool> CheckKeyExist(string key);

        Task<long?> Increment(string key, uint value);

        Task<long?> IncrementHash(string hashId, string key, uint value);

        Task<bool> IncrementHash(Dictionary<string, string> keys, uint value);

        Task<bool> IncrementHash(Dictionary<string, List<string>> keys, uint value);

        Task<long?> Decrement(string key, uint value);

        Task<long?> DecrementHash(string hashId, string key, uint value);

        Task<bool> DecrementHash(Dictionary<string, string> keys, uint value);

        Task<bool> DecrementHash(Dictionary<string, List<string>> keys, uint value);

        Task<IAsyncEnumerable<KeyValuePair<string, string>>?> ScanHash(string hashId, string pattern, int pageSize = 1000);

        Task<bool> Set<T>(string key, T value, bool compress = false);

        Task<bool> Set<T>(string key, T value, DateTime expireDate, bool compress = false);

        Task<bool> Set<T>(string key, T value, TimeSpan expireTime, bool compress = false);

        Task<bool> SetHash<T>(string hashId, string key, T value, bool compress = false);

        Task<bool> SetHash<T>(string hashId, string key, T value, int expireSecond, bool compress = false);

        Task<bool> SetJson<T>(string key, T value);

        Task<bool> SetKeyExpire(string key, DateTime expireDate);

        Task<bool> SetKeyExpire(string key, TimeSpan expireTime);

        Task<bool> SetIfNotExists<T>(string key, T value);

        Task<bool> SetIfNotExists<T>(string key, T value, int expireSecond);

        Task<bool> Remove(string key);

        Task<bool> RemoveByPattern(string pattern);

        Task<bool> RemoveByPattern(string hashId, string pattern);

        Task<bool> RemoveFromHash(string hashId, string key);

        Task<bool> RemoveFromHash(string hashId, List<string> keys);

        Task<int?> ScriptEvaluate(LuaScript script, object? parameters = null);
    }
}
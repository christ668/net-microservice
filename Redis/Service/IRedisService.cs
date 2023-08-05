using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Service
{
    public interface IRedisService
    {
        Task<string> GetCacheString(string key);
        Task<bool> CacheString(string key, string value, TimeSpan? expireTime = null);
        Task<bool> RedisHealthCheck();
        Task<bool> DeleteKey(string key);
        Task<bool> KeyExist(string key);
        Task CacheHash(string key, HashEntry[] value, TimeSpan? expireTime = null);
        Task<HashEntry[]> GetCacheHash(string key);
        Task<RedisValue> GetCacheHash(string key, string field);
        Task<bool> SetContains(string key, string value);
        Task SetAdd(string key, string value, TimeSpan? expire = null);
        IDatabase GetDatabase();
    }
}

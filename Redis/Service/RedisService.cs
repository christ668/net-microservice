using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Redis.Service
{
    public class RedisService : IRedisService
    {
        private readonly TimeSpan _defaultExpireTime = TimeSpan.FromMinutes(5);
        private readonly IConnectionMultiplexer _redisMultiplexer;
        private readonly IDatabase _redis;
        public RedisService(IConnectionMultiplexer redisMultiplexer)
        {
            _redisMultiplexer = redisMultiplexer;
            _redis = _redisMultiplexer.GetDatabase();
        }

        public async Task<bool> SetContains(string key, string value)
        {
            var result = await _redis.SetContainsAsync(key, value);
            return result;
        }

        public async Task SetAdd(string key, string value, TimeSpan? expire = null)
        {
            await _redis.SetAddAsync(key, value);
            if (expire != null)
                await _redis.KeyExpireAsync(key, expire);
        }

        public async Task<bool> RedisHealthCheck()
        {
            await CacheString($"{nameof(RedisHealthCheck)}.{nameof(RedisService)}", nameof(RedisHealthCheck));
            var result = await GetCacheString($"{nameof(RedisHealthCheck)}.{nameof(RedisService)}");

            return result.Equals(nameof(RedisHealthCheck));
        }

        public async Task<string> GetCacheString(string key)
        {
            var result = await _redis.StringGetAsync(key);
            return result;
        }

        public async Task<bool> DeleteKey(string key)
        {
            var result = await _redis.KeyDeleteAsync(key);
            return result;
        }

        public async Task<bool> CacheString(string key, string value, TimeSpan? expireTime = null)
        {
            expireTime ??= _defaultExpireTime;
            var result = await _redis.StringSetAsync(key, value, expireTime);

            return result;
        }

        public async Task CacheHash(string key, HashEntry[] value, TimeSpan? expireTime = null)
        {
            await _redis.HashSetAsync(key, value);

            if (expireTime != null)
                await _redis.KeyExpireAsync(key, expireTime);
        }

        public async Task<HashEntry[]> GetCacheHash(string key)
        {
            var result = await _redis.HashGetAllAsync(key);
            return result;
        }

        public async Task<bool> KeyExist(string key)
        {
            var result = await _redis.KeyExistsAsync(key);
            return result;
        }

        public async Task<RedisValue> GetCacheHash(string key, string field)
        {
            var result = await _redis.HashGetAsync(key, field);
            return result;
        }

        public IDatabase GetDatabase()
        {
            var result = _redis;
            return result;
        }
    }
}

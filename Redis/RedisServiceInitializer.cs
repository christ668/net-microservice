using Microsoft.Extensions.DependencyInjection;
using Redis.Service;
using StackExchange.Redis;

namespace Redis
{
    public static class RedisServiceInitializer
    {
        public static IServiceCollection UseRedis(this IServiceCollection services, string redisConnection)
        {
            services.AddSingleton<IConnectionMultiplexer>(t => ConnectionMultiplexer.Connect(redisConnection));
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
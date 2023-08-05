using Microsoft.Extensions.DependencyInjection;
using Redis.Service;
using StackExchange.Redis;

namespace common
{
    public static class RedisServiceInitializer
    {
        public static IServiceCollection UseAgateRedis(this IServiceCollection services, string redisConnection)
        {
            services.AddSingleton<IConnectionMultiplexer>(t => ConnectionMultiplexer.Connect(redisConnection));
            services.AddScoped<IRedisService, RedisService>();
            return services;
        }
    }
}
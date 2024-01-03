using Application.Abstractions.Cache;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.MsgPack;

namespace WebAPI.Extensions;

public static class CacheExtensions
{
    public static void AddStackExchangeRedis(this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection("Redis:Configuration").Get<RedisConfiguration>();

        services.AddSingleton(redisConfiguration!);

        services.AddSingleton<IRedisClient, RedisClient>();
        services.AddSingleton<IRedisConnectionPoolManager, RedisConnectionPoolManager>();
        services.AddSingleton<ISerializer, MsgPackObjectSerializer>();

        services.AddSingleton<IRedisCacheService>((provider) =>
            (provider.GetRequiredService<IRedisClient>().GetDefaultDatabase().Database as IRedisCacheService)!);
    }
}
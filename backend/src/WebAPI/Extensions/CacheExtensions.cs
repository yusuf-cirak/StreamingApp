using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace WebAPI.Extensions;

/// <summary>
/// Cache extensions for the application.
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    /// Extension method for registering Redis configurations and services to the application.
    /// </summary>
    /// <param name="services">IServiceCollection interface reference for extension.</param>
    /// <param name="configuration">IConfiguration for reading application configurations.</param>
    public static void AddStackExchangeRedis(this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetSection("Redis:Configuration").Get<RedisConfiguration>();

        services.AddSingleton(redisConfiguration!);

        services.AddSingleton<IRedisClient, RedisClient>();
        services.AddSingleton<IRedisConnectionPoolManager, RedisConnectionPoolManager>();
        services.AddSingleton<ISerializer, NewtonsoftSerializer>();

        services.AddSingleton<IRedisDatabase>((provider) =>
            (provider.GetRequiredService<IRedisClient>().GetDefaultDatabase()));
    }
}
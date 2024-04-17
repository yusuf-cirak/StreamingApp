using Application.Abstractions.Caching;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Infrastructure.Services.Cache
{
    public sealed class RedisCacheService : ICacheService
    {
        private readonly IRedisDatabase _redisDb;
        private readonly int _defaultExpiration;

        public RedisCacheService(IRedisDatabase redisDb, IConfiguration configuration)
        {
            _redisDb = redisDb ?? throw new ArgumentNullException(nameof(redisDb));
            _defaultExpiration = configuration.GetSection("Redis:Configuration:DefaultExpiration").Get<int?>() ?? 15;
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            RedisValue bytes = await _redisDb.Database.StringGetAsync(key);
            if (bytes.IsNullOrEmpty)
            {
                return default;
            }

            return _redisDb.Serializer.Deserialize<T>(bytes);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiresIn = null,
            CancellationToken cancellationToken = default)
        {
            expiresIn ??= TimeSpan.FromMinutes(_defaultExpiration);
            var value = await GetAsync<T>(key, cancellationToken);

            if (value == null)
            {
                value = await factory();
                await SetAsync(key, value, expiresIn, cancellationToken: cancellationToken);
            }

            return value;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiresIn = null,
            CancellationToken cancellationToken = default)
        {
            expiresIn ??= TimeSpan.FromMinutes(_defaultExpiration);

            var valueBytes = _redisDb.Serializer.Serialize(value);
            return await _redisDb.Database.StringSetAsync(key, valueBytes, expiresIn, When.Always);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _redisDb.Database.KeyDeleteAsync(key);
        }
    }
}
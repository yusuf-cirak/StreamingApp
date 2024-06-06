using Application.Abstractions.Caching;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Infrastructure.Services.Cache
{
    public sealed class RedisCacheService : IRedisCacheService
    {
        public IRedisDatabase RedisDb { get; }

        public RedisCacheService(IRedisDatabase redisDb)
        {
            RedisDb = redisDb ?? throw new ArgumentNullException(nameof(redisDb));
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            RedisValue bytes = await RedisDb.Database.StringGetAsync(key);
            if (bytes.IsNullOrEmpty)
            {
                return default;
            }

            return RedisDb.Serializer.Deserialize<T>(bytes);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiresIn = null,
            CancellationToken cancellationToken = default)
        {
            var value = await GetAsync<T>(key, cancellationToken);

            if (value == null)
            {
                value = await factory();
                await SetAsync<T>(key, value, expiresIn, cancellationToken: cancellationToken);
            }

            return value;
        }

        public async Task<T> GetOrUseFactoryAsync<T>(string key, Func<Task<T>> factory,
            CancellationToken cancellationToken = default)
            => await GetAsync<T>(key, cancellationToken) ?? await factory();


        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiresIn = null,
            CancellationToken cancellationToken = default)
        {
            var valueBytes = RedisDb.Serializer.Serialize(value);
            return await RedisDb.Database.StringSetAsync(key, valueBytes, expiresIn, When.Always);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await RedisDb.Database.KeyDeleteAsync(key);
        }
    }
}
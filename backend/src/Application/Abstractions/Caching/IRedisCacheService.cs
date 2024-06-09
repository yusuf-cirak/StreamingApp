using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Abstractions.Caching;

public interface IRedisCacheService : ICacheService
{
    public IRedisDatabase RedisDb { get;  }
}
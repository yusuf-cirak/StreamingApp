namespace Application.Abstractions.Caching;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default);

    Task<T> GetOrUseFactoryAsync<T>(string key, Func<Task<T>> factory,
        CancellationToken cancellationToken = default);

    Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiresIn = null,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveAsync(string key);

    Task<bool> TakeLockAsync(string key, TimeSpan expiration);
    Task<bool> ReleaseLockAsync(string key);
}
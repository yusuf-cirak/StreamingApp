using Application.Abstractions.Caching;
using Application.Abstractions.Locking;
using Application.Common.Errors;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Common.Behaviors;

public sealed class LockBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILockRequest
    where TResponse : IHttpResult, new()
{
    private readonly ICacheService cacheService;

    public LockBehavior(IRedisDatabase cacheService)
    {
        cacheService = cacheService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var acquireLock = await cacheService.TakeLockAsync(request.Key, TimeSpan.FromSeconds(request.Expiration));
        try
        {
            if (acquireLock is false)
            {
                var response = new TResponse().CreateWith(
                    AuthorizationErrors.Forbidden,
                    StatusCodes.Status403Forbidden);

                return (TResponse)response;
            }

            return await next();
        }
        finally
        {
            if (acquireLock is true && request.ReleaseImmediately is true)
            {
                await cacheService.ReleaseLockAsync(request.Key);
            }
        }
    }
}
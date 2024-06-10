using Application.Abstractions.Locking;
using Application.Common.Errors;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Application.Common.Behaviors;

public sealed class LockBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILockRequest
    where TResponse : IHttpResult, new()
{
    private readonly IRedisDatabase _redisDb;

    public LockBehavior(IRedisDatabase redisDb)
    {
        _redisDb = redisDb;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var acquireLock = await _redisDb.Database.LockTakeAsync(request.Key, true, TimeSpan.FromSeconds(request.Expiration));
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
              await  _redisDb.Database.LockReleaseAsync(request.Key, true);
            }
        }
    }
}
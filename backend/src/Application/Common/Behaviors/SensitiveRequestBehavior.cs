using Application.Common.Errors;
using Application.Common.Services;
using Application.Features.Users.Services;

namespace Application.Common.Behaviors;

public sealed class SensitiveRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISensitiveRequest
    where TResponse : IHttpResult, new()
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserBlacklistManager _blacklistManager;

    public SensitiveRequestBehavior(IUserBlacklistManager blacklistManager, ICurrentUserService currentUserService)
    {
        _blacklistManager = blacklistManager;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId.ToString();

        var isBlacklisted = await _blacklistManager.IsUserBlacklistedAsync(userId);

        if (isBlacklisted)
        {
            var response = new TResponse().CreateWith(
                AuthorizationErrors.Unauthorized(),
                StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        return await next();
    }
}
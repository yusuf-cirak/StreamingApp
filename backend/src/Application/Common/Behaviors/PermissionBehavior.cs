using Application.Common.Errors;

namespace Application.Common.Behaviors;

public sealed class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IPermissionRequest
    where TResponse : IHttpResult, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationResult = request.PermissionRequirements.Authorize(_httpContextAccessor.HttpContext!.User);

        if (authorizationResult.IsFailure)
        {
            var response = new TResponse().CreateWith(
                AuthorizationErrors.Unauthorized(authorizationResult.Error.Message),
                StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        return await next();
    }
}
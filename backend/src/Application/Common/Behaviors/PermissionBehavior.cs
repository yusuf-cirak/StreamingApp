using Application.Common.Errors;
using Application.Common.Permissions;

namespace Application.Common.Behaviors;

public sealed class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IPermissionRequest
    where TResponse : IHttpResult, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPermissionService _permissionService;

    public PermissionBehavior(IHttpContextAccessor httpContextAccessor, IPermissionService permissionService)
    {
        _httpContextAccessor = httpContextAccessor;
        _permissionService = permissionService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;

        var authorizationResult = _permissionService.Authorize(request.PermissionRequirements, user);

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
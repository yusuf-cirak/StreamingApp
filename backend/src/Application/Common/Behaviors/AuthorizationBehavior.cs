using System.Text;
using Application.Common.Errors;

namespace Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
    where TResponse : IHttpResult, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var claims = _httpContextAccessor.HttpContext?.User.Claims.ToList();

        if (claims is null or { Count: 0 })
        {
            var response = new TResponse().CreateWith(AuthorizationErrors.Unauthorized(),
                StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        var authorizationResult = request.PermissionRequirements.Authorize(_httpContextAccessor.HttpContext.User);

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
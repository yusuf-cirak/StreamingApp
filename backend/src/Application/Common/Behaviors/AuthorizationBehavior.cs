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

        var authorizationFailureResults = request.AuthorizationRules
            .Select(rule => rule(claims, request))
            .Where(result => result.IsFailure)
            .ToList();

        if (authorizationFailureResults.Count > 0)
        {
            StringBuilder sb = new();

            authorizationFailureResults.ForEach(result => sb.AppendLine(result.Error.Message));

            var response = new TResponse().CreateWith(AuthorizationErrors.Unauthorized(sb.ToString()),
                StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        return await next();
    }
}
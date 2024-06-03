using System.Text;
using Application.Common.Errors;

namespace Application.Common.Behaviors;

public sealed class ApiAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IApiSecuredRequest
    where TResponse : IHttpResult, new()
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var authorizationFailureResults = request.AuthorizationRequirements
            .Select(rule => rule(httpContext, ArraySegment<Claim>.Empty, request))
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
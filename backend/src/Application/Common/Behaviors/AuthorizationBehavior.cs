using System.Reflection;
using Application.Common.Errors;
using Application.Common.Extensions;

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
        HashSet<string> roleClaimsFromToken =
            (_httpContextAccessor.HttpContext?.User?.ClaimRoles() ?? new()).ToHashSet();

        if (roleClaimsFromToken.Count == 0)
        {
            var response =
                new TResponse().CreateWith(AuthorizationErrors.Unauthorized, StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        HashSet<string> roleClaimsFromAttribute =
            (request.GetType().GetCustomAttribute<AuthorizationPipelineAttribute>()?.Roles ?? []).ToHashSet();

        if (roleClaimsFromAttribute.Count == 0)
        {
            var response =
                new TResponse().CreateWith(AuthorizationErrors.Unauthorized, StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        var isNotMatchedARoleClaimWithRequestRoles = roleClaimsFromToken
            .Except(roleClaimsFromAttribute).Any();


        if (isNotMatchedARoleClaimWithRequestRoles)
        {
            var response =
                new TResponse().CreateWith(AuthorizationErrors.Unauthorized, StatusCodes.Status401Unauthorized);

            return (TResponse)response;
        }

        return await next();
    }
}
using System.Reflection;
using Application.Common.Exceptions;
using Application.Common.Extensions;

namespace Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
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
            throw new AuthorizationException("You are not authorized to access this resource.");
        }

        HashSet<string> roleClaimsFromAttribute =
            (request.GetType().GetCustomAttribute<AuthorizationPipelineAttribute>()?.Roles ?? []).ToHashSet();

        if (roleClaimsFromAttribute.Count == 0)
        {
            throw new AuthorizationException("You are not authorized to access this resource.");
        }

        var isNotMatchedARoleClaimWithRequestRoles = roleClaimsFromToken
            .Except(roleClaimsFromAttribute).Any();


        if (isNotMatchedARoleClaimWithRequestRoles)
        {
            throw new AuthorizationException("You are not authorized to access this resource.");
        }

        TResponse response = await next();

        return response;
    }
}
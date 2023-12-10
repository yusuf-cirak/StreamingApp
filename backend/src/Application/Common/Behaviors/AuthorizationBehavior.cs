using Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var userClaimsCount = _httpContextAccessor.HttpContext?.User?.Claims?.Count() ?? 0;

        if (userClaimsCount == 0)
        {
            throw new UnauthorizedAccessException("You are not authorized to access this resource.");
        }

        // TODO: Role based authorization

        TResponse response = await next();

        return response;
    }
}
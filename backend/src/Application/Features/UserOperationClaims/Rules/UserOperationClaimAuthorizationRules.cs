using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Features.UserOperationClaims.Abstractions;

namespace Application.Features.UserOperationClaims.Rules;

public static class UserOperationClaimAuthorizationRules
{
    public static Result CanUserCreateOrDeleteUserOperationClaim(HttpContext context, ICollection<Claim> claims,
        object request)
    {
        if (IsUserAdmin(claims))
        {
            return Result.Success();
        }

        if (IsUserStreamerForRequestedStream(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(AuthorizationErrors.Unauthorized());
    }


    private static bool IsUserAdmin(ICollection<Claim> claims)
    {
        var roles = claims.GetRoles().ToList();

        return roles.Exists(rc => rc.Name == RoleConstants.Admin);
    }


    private static bool IsUserStreamerForRequestedStream(ICollection<Claim> claims, object request)
    {
        var userIdFromClaim =
            Guid.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        var valueFromRequest = Guid.Parse(((IUserOperationClaimCommandRequest)request).Value);

        // This checks that if the user is trying to create a user operation claim its own stream
        return userIdFromClaim == valueFromRequest;
    }
}
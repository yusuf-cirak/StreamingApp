using System.Text.Json;
using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Contracts.Roles;
using Application.Features.UserRoleClaims.Abstractions;

namespace Application.Features.UserRoleClaims.Rules;

public static class UserRoleClaimAuthorizationRules
{
    public static Result CanUserCreateOrDeleteUserRoleClaim(HttpContext context, ICollection<Claim> claims,
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
        var roles = claims.GetRoles();

        return roles.Any(rc => rc.Name == RoleConstants.SystemAdmin);
    }


    private static bool IsUserStreamerForRequestedStream(ICollection<Claim> claims, object request)
    {
        var userIdFromClaim =
            Guid.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        var valueFromRequest = Guid.Parse(((IUserRoleClaimCommandRequest)request).Value);

        // This checks that if the user is trying to create a user operation claim its own stream
        return userIdFromClaim == valueFromRequest;
    }
}
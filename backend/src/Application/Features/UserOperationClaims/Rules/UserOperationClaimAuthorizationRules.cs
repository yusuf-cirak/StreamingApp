using System.Text.Json;
using Application.Common.Errors;
using Application.Contracts.Roles;
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
        string rolesString = claims.FirstOrDefault(claim => claim.Type == "Roles")?.Value ??
                             string.Empty;

        if (string.IsNullOrEmpty(rolesString))
        {
            return false;
        }

        List<GetUserRoleDto> roleClaims = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);

        return roleClaims.Exists(rc => rc.Role.Name == RoleConstants.SystemAdmin);
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
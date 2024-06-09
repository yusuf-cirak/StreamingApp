using Application.Common.Errors;
using Application.Common.Extensions;
using Application.Features.Users.Abstractions;

namespace Application.Features.Users.Rules;

public static class UserAuthorizationRules
{
    public static Result CanUpdateUser(HttpContext context, IEnumerable<Claim> claims, object request)
    {
        if (IsAdmin(claims, request) || IsOwner(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(AuthorizationErrors.Unauthorized());
    }

    private static bool IsAdmin(IEnumerable<Claim> claims, object request)
    {
        var roles = claims.GetRoles();
        return roles.Any(ur => ur.Name == RoleConstants.Admin);
    }

    private static bool IsOwner(IEnumerable<Claim> claims, object request)
    {
        var userId = Guid.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        var requestUserId = ((IUserCommandRequest)request).UserId;

        return userId == requestUserId;
    }
}
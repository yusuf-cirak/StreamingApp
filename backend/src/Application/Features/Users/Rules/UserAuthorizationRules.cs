using System.Text.Json;
using Application.Common.Errors;
using Application.Contracts.Roles;
using Application.Features.Users.Abstractions;

namespace Application.Features.Users.Rules;

public static class UserAuthorizationRules
{
    public static Result CanUpdateUser(HttpContext context, ICollection<Claim> claims, object request)
    {
        if (IsAdmin(claims, request) || IsOwner(claims, request))
        {
            return Result.Success();
        }

        return Result.Failure(AuthorizationErrors.Unauthorized());
    }

    private static bool IsAdmin(ICollection<Claim> claims, object request)
    {
        var rolesString = claims.FirstOrDefault(c => c.Type == "Roles")?.Value ?? string.Empty;
        var roles = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);
        return roles.Any(ur => ur.Role.Name == RoleConstants.SystemAdmin);
    }

    private static bool IsOwner(ICollection<Claim> claims, object request)
    {
        var userId = Guid.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        var requestUserId = ((IUserCommandRequest)request).UserId;

        return userId == requestUserId;
    }
}
using System.Text.Json;
using Application.Common.Errors;
using Application.Contracts.Roles;

namespace Application.Common.Rules;

public static class CommonAuthorizationRules
{
    public static Result UserMustBeAdmin(HttpContext context, ICollection<Claim> claims, object request)
    {
        var rolesString = claims.FirstOrDefault(claim => claim.Type == "Roles")?.Value ?? String.Empty;

        if (string.IsNullOrEmpty(rolesString))
        {
            return Result.Failure(RoleErrors.DoesNotExist);
        }

        var roles = JsonSerializer.Deserialize<List<GetUserRoleDto>>(rolesString);

        if (!roles.Exists(r => r.Name.Name == RoleConstants.SystemAdmin))
        {
            return Result.Failure(AuthorizationErrors.Unauthorized());
        }

        return Result.Success();
    }
}
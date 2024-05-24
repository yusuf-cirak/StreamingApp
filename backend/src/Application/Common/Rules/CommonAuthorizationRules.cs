using Application.Common.Errors;
using Application.Common.Extensions;

namespace Application.Common.Rules;

public static class CommonAuthorizationRules
{
    public static Result UserMustBeAdmin(HttpContext context, ICollection<Claim> claims, object request)
    {
        var roles = claims.GetRoles().ToList();

        if (!roles.Exists(r => r.Name == RoleConstants.SystemAdmin))
        {
            return Result.Failure(AuthorizationErrors.Unauthorized());
        }

        return Result.Success();
    }
}
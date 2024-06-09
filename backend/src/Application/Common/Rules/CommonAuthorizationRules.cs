using Application.Common.Errors;
using Application.Common.Extensions;

namespace Application.Common.Rules;

public static class CommonAuthorizationRules
{
    public static Result UserMustBeAdmin(HttpContext context, ICollection<Claim> claims, object request)
    {
        var roles = claims.GetRoles();

        if (roles.All(r => r.Name != RoleConstants.Admin))
        {
            return Result.Failure(AuthorizationErrors.Unauthorized());
        }

        return Result.Success();
    }
}
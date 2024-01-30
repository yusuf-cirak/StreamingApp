using System.Security.Claims;

namespace Application.Common.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal?.Claims(ClaimTypes.Role);

    public static string GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.SingleOrDefault() ?? string.Empty;
}
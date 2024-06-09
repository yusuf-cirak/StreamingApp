using System.Security.Claims;

namespace SharedKernel;

public static partial class ClaimsPrincipalExtensions
{
    public static Claim Claim(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal?.FindFirst(claimType);
    }

    public static IEnumerable<Claim> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal?.FindAll(claimType);
    }

    public static IEnumerable<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.Claims(ClaimTypes.Role).Select(c => c.Value);

    public static string GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.Claims(ClaimTypes.NameIdentifier)?.Select(c => c.Value).SingleOrDefault();
}
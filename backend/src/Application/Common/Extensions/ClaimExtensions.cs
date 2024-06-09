using System.Text.Json;

namespace Application.Common.Extensions;

public static partial class ClaimsPrincipalExtensions
{
    private static readonly string EmptyStringArray = "[]";

    public static string GetClaim(this IEnumerable<Claim> claims, string claimType,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return claims.FirstOrDefault(c => c.Type.Equals(claimType, comparison))?.Value;
    }

    public static IEnumerable<Claim> GetClaims(this IEnumerable<Claim> claims, string claimType,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        return claims
            .Where(c => c.Type.Equals(claimType, comparison));
    }

    public static IEnumerable<GetUserRoleDto> GetRoles(this IEnumerable<Claim> claims) =>
        JsonSerializer.Deserialize<IEnumerable<GetUserRoleDto>>(claims.GetClaim(ClaimTypes.Role) ?? EmptyStringArray);

    public static IEnumerable<GetUserOperationClaimDto> GetOperationClaims(this IEnumerable<Claim> claims) =>
        JsonSerializer.Deserialize<IEnumerable<GetUserOperationClaimDto>>(claims.GetClaim("operationClaims") ??
                                                                          EmptyStringArray);
}
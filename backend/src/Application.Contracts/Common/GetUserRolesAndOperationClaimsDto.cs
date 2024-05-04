using Application.Contracts.OperationClaims;
using Application.Contracts.Roles;

namespace Application.Contracts.Common;

public record GetUserRolesAndOperationClaimsDto(
    Guid UserId,
    IEnumerable<GetUserRoleDto> Roles,
    IEnumerable<GetUserOperationClaimDto> OperationClaims
);
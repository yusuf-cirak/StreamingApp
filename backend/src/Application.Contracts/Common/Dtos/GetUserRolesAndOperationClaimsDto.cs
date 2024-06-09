using Application.Contracts.OperationClaims;
using Application.Contracts.Roles;

namespace Application.Contracts.Common.Dtos;

public record GetUserRolesAndOperationClaimsDto(
    Guid UserId,
    IEnumerable<GetUserRoleDto> Roles,
    IEnumerable<GetUserOperationClaimDto> OperationClaims
);
using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;

namespace Application.Common.Dtos;

public readonly record struct GetUserRolesAndOperationClaimsDto(
    Guid UserId,
    IEnumerable<GetUserRoleDto> Roles,
    IEnumerable<GetUserOperationClaimDto> OperationClaims
);
using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;

namespace Application.Common.Dtos;

public record GetUserRolesAndOperationClaimsDto(
    Guid UserId,
    IEnumerable<GetUserRoleDto> Roles,
    IEnumerable<GetUserOperationClaimDto> OperationClaims
);
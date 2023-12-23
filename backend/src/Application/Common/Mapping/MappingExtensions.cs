using Application.Features.OperationClaims.Dtos;
using Application.Features.Roles.Dtos;

namespace Application.Common.Mapping;

public static class MappingExtensions
{
    public static GetOperationClaimDto ToDto(this OperationClaim operationClaim) =>
        new(operationClaim.Id, operationClaim.Name);

    public static GetRoleDto ToDto(this Role role) => new (role.Id, role.Name);
}
namespace Application.Features.RoleOperationClaims.Dtos;

public readonly record struct GetRoleOperationClaimDto(Guid RoleId, Guid OperationClaimId);
namespace Application.Contracts.RoleOperationClaims;

public readonly record struct GetRoleOperationClaimDto(Guid RoleId, Guid OperationClaimId);
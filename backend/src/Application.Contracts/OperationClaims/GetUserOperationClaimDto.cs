namespace Application.Contracts.OperationClaims;

public readonly record struct GetUserOperationClaimDto(GetOperationClaimDto OperationClaim, string Value);
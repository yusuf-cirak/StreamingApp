namespace Application.Features.OperationClaims.Dtos;

public readonly record struct GetUserOperationClaimDto(GetOperationClaimDto OperationClaim, string Value);
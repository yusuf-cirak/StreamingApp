namespace Application.Features.UserOperationClaims.Abstractions;

public interface IUserOperationClaimCommandRequest
{
    public Guid UserId { get; init; }
    public Guid OperationClaimId { get; init; }
    public string Value { get; set; }
}
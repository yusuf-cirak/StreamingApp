namespace Domain.Entities;

public class UserOperationClaim : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }

    public virtual User User { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }

    public UserOperationClaim(Guid userId, Guid operationClaimId)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }

    private UserOperationClaim()
    {
    }

    public static UserOperationClaim Create(Guid userId, Guid operationClaimId)
    {
        return new(userId, operationClaimId);
    }
}
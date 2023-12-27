namespace Domain.Entities;

public class UserOperationClaim : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }

    public string Value { get; set; }

    public virtual User User { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }

    public UserOperationClaim(Guid userId, Guid operationClaimId, string value)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
        Value = value;
    }

    private UserOperationClaim()
    {
    }

    public static UserOperationClaim Create(Guid userId, Guid operationClaimId, string value)
    {
        return new(userId, operationClaimId, value);
    }
}
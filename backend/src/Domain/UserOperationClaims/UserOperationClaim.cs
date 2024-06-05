namespace Domain.Entities;

public class UserOperationClaim : BaseEntity, IEquatable<UserOperationClaim>
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

    public UserOperationClaim()
    {
    }

    public static UserOperationClaim Create(Guid userId, Guid operationClaimId, string value)
    {
        return new(userId, operationClaimId, value);
    }

    public bool Equals(UserOperationClaim other)
    {
        if (other == null) return false;
        return this.OperationClaimId == other.OperationClaimId && this.UserId == other.UserId &&
               this.Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is UserOperationClaim other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserId, OperationClaimId, Value);
    }
}
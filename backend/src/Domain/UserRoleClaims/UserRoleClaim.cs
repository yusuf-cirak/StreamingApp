namespace Domain.Entities;

public class UserRoleClaim : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public string Value { get; set; }

    public virtual User User { get; set; }
    public virtual Role Role { get; set; }

    private UserRoleClaim()
    {
    }

    private UserRoleClaim(Guid userId, Guid roleId, string value)
    {
        UserId = userId;
        RoleId = roleId;
        Value = value;
    }

    public static UserRoleClaim Create(Guid userId, Guid roleOperationClaimId, string value)
    {
        UserRoleClaim userOperationClaim = new(userId, roleOperationClaimId, value);
        // userOperationClaim.Raise(new UserOperationClaimCreatedEvent(userOperationClaim));
        return userOperationClaim;
    }
}
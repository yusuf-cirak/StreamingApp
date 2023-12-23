﻿namespace Domain.Entities;

public class UserRoleClaim : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }

    private UserRoleClaim()
    {
    }

    private UserRoleClaim(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public static UserRoleClaim Create(Guid userId, Guid roleOperationClaimId)
    {
        UserRoleClaim userOperationClaim = new(userId, roleOperationClaimId);
        // userOperationClaim.Raise(new UserOperationClaimCreatedEvent(userOperationClaim));
        return userOperationClaim;
    }
}
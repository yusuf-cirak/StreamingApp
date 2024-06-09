namespace Domain.Entities;

public class RoleOperationClaim : EquatableEntity
{
    public Guid RoleId { get; set; }
    public Guid OperationClaimId { get; set; }

    public virtual Role Role { get; set; }
    public virtual OperationClaim OperationClaim { get; set; }

    private RoleOperationClaim()
    {
    }

    private RoleOperationClaim(Guid roleOperationClaimId, Guid operationClaimId)
    {
        RoleId = roleOperationClaimId;
        OperationClaimId = operationClaimId;
    }

    private RoleOperationClaim(Guid id, Guid roleOperationClaimId, Guid operationClaimId)
    {
        RoleId = roleOperationClaimId;
        OperationClaimId = operationClaimId;
    }

    public static RoleOperationClaim Create(Guid roleOperationClaimId, Guid operationClaimId)
    {
        var roleOperationClaim = new RoleOperationClaim(roleOperationClaimId, operationClaimId);
        //userRole.Raise(new UserRoleCreatedEvent(userRole));
        return roleOperationClaim;
    }
}
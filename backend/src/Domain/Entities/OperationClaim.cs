namespace Domain.Entities;

public class OperationClaim : Entity
{
    public string Name { get; }

    private OperationClaim()
    {
    }

    private OperationClaim(string name)
    {
        Name = name;
    }

    public static OperationClaim Create(string name)
    {
        var operationClaim = new OperationClaim(name);
        // operationClaim.Raise(new OperationClaimCreatedEvent(operationClaim));
        return operationClaim;
    }
}
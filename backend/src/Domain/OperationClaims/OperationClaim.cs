namespace Domain.Entities;

public class OperationClaim : Entity
{
    public string Name { get; set; }

    private OperationClaim()
    {
    }

    private OperationClaim(string name)
    {
        Name = name;
    }

    private OperationClaim(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static OperationClaim Create(string name)
    {
        var operationClaim = new OperationClaim(name);
        // operationClaim.Raise(new OperationClaimCreatedEvent(operationClaim));
        return operationClaim;
    }

    public static OperationClaim Create(Guid id, string name)
    {
        var operationClaim = new OperationClaim(id, name);
        // operationClaim.Raise(new OperationClaimCreatedEvent(operationClaim));
        return operationClaim;
    }
}
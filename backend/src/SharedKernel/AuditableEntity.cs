namespace SharedKernel;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}

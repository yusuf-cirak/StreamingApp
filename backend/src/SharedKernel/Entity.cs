using System.Collections.Immutable;

namespace SharedKernel;
public abstract class Entity<T>
{
    public T Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();


    protected Entity()
    {

    }

    protected Entity(T id)
    {
        Id = id;
    }


    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

}

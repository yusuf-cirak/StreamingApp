using System.Collections.Immutable;

namespace SharedKernel;
public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid().ToShortGuid();

    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity()
    {

    }

    public IImmutableList<IDomainEvent> DomainEvents => _domainEvents.ToImmutableList();

    protected void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

}

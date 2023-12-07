using System.Collections.Immutable;

namespace SharedKernel;
public abstract class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid().ToShortGuid();

    private readonly List<IBaseDomainEvent> _domainEvents = new();

    protected Entity()
    {

    }

    public IImmutableList<IBaseDomainEvent> DomainEvents => _domainEvents.ToImmutableList();

    protected void Raise(IBaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

}

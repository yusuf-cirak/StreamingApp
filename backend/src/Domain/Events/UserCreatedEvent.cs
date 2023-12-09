
namespace Domain.Events;
public readonly record struct UserCreatedEvent(User User) : IBaseDomainEvent;

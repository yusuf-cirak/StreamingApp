namespace Domain.Users;
public readonly record struct UserCreatedEvent(User User) : IDomainEvent;

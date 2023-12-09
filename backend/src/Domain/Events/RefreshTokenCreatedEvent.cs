namespace Domain.Events;

public readonly record struct RefreshTokenCreatedEvent(RefreshToken RefreshToken) : IBaseDomainEvent;


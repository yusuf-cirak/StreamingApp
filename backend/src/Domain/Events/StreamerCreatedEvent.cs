namespace Domain.Events;

public readonly record struct StreamerCreatedEvent(Streamer Streamer) : IDomainEvent;


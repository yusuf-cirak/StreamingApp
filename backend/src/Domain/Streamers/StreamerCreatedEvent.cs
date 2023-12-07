namespace Domain.Streamers;

public readonly record struct StreamerCreatedEvent(Streamer Streamer) : IDomainEvent;


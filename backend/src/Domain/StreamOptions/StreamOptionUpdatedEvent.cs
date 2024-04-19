using MediatR;

namespace Domain.Events;

public readonly record struct StreamOptionUpdatedEvent(StreamOption StreamOption) : IDomainEvent,INotification;
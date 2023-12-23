namespace Domain.Events;
public readonly record struct OperationClaimCreatedEvent(OperationClaim OperationClaim) : IDomainEvent;

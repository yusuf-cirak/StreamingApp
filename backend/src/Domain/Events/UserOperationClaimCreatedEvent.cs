
namespace Domain.Events;

public readonly record struct UserOperationClaimCreatedEvent(UserRoleClaim UserOperationClaim) : IDomainEvent;

namespace Domain.Events;

public readonly record struct UserRoleClaimCreatedEvent(UserRoleClaim UserOperationClaim) : IDomainEvent;
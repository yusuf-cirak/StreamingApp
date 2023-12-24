using Application.Abstractions.Security;

namespace Application.Features.StreamBlockedUsers.Commands.Create;

[AuthorizationPipeline(Roles = ["Admin", "Streamer", "Moderator"])]
public readonly record struct StreamBlockedUserCreateCommandRequest(Guid StreamId, Guid BlockedUserId)
    : IRequest<HttpResult>, ISecuredRequest;
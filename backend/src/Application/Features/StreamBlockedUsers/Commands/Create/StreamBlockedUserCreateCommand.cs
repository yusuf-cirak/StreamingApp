using Application.Features.StreamBlockedUsers.Rules;

namespace Application.Features.StreamBlockedUsers.Commands.Create;

[AuthorizationPipeline(Roles = ["Admin", "Streamer", "Moderator"])]
public readonly record struct StreamBlockedUserCreateCommandRequest(Guid StreamerId, Guid BlockedUserId)
    : IRequest<HttpResult>, ISecuredRequest;

public sealed class StreamBlockedUserCreateHandler : IRequestHandler<StreamBlockedUserCreateCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly StreamBlockedUserBusinessRules _businessRules;

    public StreamBlockedUserCreateHandler(IEfRepository efRepository, StreamBlockedUserBusinessRules businessRules)
    {
        _efRepository = efRepository;
        _businessRules = businessRules;
    }

    public async Task<HttpResult> Handle(StreamBlockedUserCreateCommandRequest request,
        CancellationToken cancellationToken)
    {
        var verifyBlockerUser =
            await _businessRules.BlockerUserShouldBeAdminOrModerator(request.StreamerId, request.StreamerId);

        if (verifyBlockerUser.IsFailure)
        {
            return HttpResult.Failure(verifyBlockerUser.Error);
        }

        var streamBlockedUser = StreamBlockedUser.Create(request.StreamerId, request.BlockedUserId);

        _efRepository.StreamBlockedUsers.Add(streamBlockedUser);

        await _efRepository.SaveChangesAsync(cancellationToken);
        
        return HttpResult.Success();
    }
}
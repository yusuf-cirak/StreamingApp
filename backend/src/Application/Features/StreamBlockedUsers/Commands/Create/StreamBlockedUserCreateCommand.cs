using Application.Features.StreamBlockedUsers.Abstractions;
using Application.Features.StreamBlockedUsers.Rules;

namespace Application.Features.StreamBlockedUsers.Commands.Create;

public readonly record struct CreateStreamBlockedUserCreateCommandRequest
    : IStreamBlockedUserRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }

    public Guid BlockedUserId { get; init; }
    public List<Func<ICollection<Claim>, object, Result>> AuthorizationRules { get; }

    public CreateStreamBlockedUserCreateCommandRequest()
    {
        AuthorizationRules = [StreamBlockedUserAuthorizationRules.CanUserBlockOrUnblockAUserFromStream];
    }

    public CreateStreamBlockedUserCreateCommandRequest(Guid streamerId, Guid blockedUserId) : this()
    {
        StreamerId = streamerId;
        BlockedUserId = blockedUserId;
    }
}

public sealed class
    CreateStreamBlockedUserCreateHandler : IRequestHandler<CreateStreamBlockedUserCreateCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly StreamBlockedUserBusinessRules _streamBlockedUserBusinessRules;

    public CreateStreamBlockedUserCreateHandler(IEfRepository efRepository,
        StreamBlockedUserBusinessRules streamBlockedUserBusinessRules)
    {
        _efRepository = efRepository;
        _streamBlockedUserBusinessRules = streamBlockedUserBusinessRules;
    }

    public async Task<HttpResult> Handle(CreateStreamBlockedUserCreateCommandRequest request,
        CancellationToken cancellationToken)
    {
        var streamBlockedUserExistResult =
            await _streamBlockedUserBusinessRules.BlockedUserIsNotAlreadyBlocked(request.StreamerId,
                request.BlockedUserId, cancellationToken);

        if (streamBlockedUserExistResult.IsFailure)
        {
            return streamBlockedUserExistResult.Error;
        }

        var streamBlockedUser = StreamBlockedUser.Create(request.StreamerId, request.BlockedUserId);

        _efRepository.StreamBlockedUsers.Add(streamBlockedUser);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status201Created)
            : StreamBlockedUserErrors.FailedToBlockUser;
    }
}
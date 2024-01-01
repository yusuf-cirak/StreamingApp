using Application.Features.StreamFollowerUsers.Abstractions;
using Application.Features.StreamFollowerUsers.Rules;

namespace Application.Features.StreamFollowerUsers.Commands.Delete;

public readonly record struct DeleteStreamFollowerUserCommandRequest : IStreamFollowerUserCommandRequest,
    IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }

    public AuthorizationFunctions AuthorizationFunctions { get; }

    public DeleteStreamFollowerUserCommandRequest()
    {
        AuthorizationFunctions = [StreamFollowerUserAuthorizationRules.CanUserFollowStreamer];
    }

    public DeleteStreamFollowerUserCommandRequest(Guid streamerId, Guid userId) : this()
    {
        StreamerId = streamerId;
        UserId = userId;
    }
}

public sealed class
    DeleteStreamFollowerUserCommandHandler : IRequestHandler<DeleteStreamFollowerUserCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteStreamFollowerUserCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult> Handle(DeleteStreamFollowerUserCommandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _efRepository
            .StreamFollowerUsers
            .Where(sfu => sfu.UserId == request.UserId && sfu.StreamerId == request.StreamerId)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamFollowerUserErrors.FailedToRemoveFollowerUserFromStream,
                StatusCodes.Status400BadRequest);
    }
}
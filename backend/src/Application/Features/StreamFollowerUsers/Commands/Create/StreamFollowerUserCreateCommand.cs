using System.Security.Claims;
using Application.Features.Streamers.Abstractions;
using Application.Features.StreamFollowerUsers.Abstractions;
using Application.Features.StreamFollowerUsers.Rules;

namespace Application.Features.StreamFollowerUsers.Commands.Create;

public readonly record struct StreamFollowerUserCreateCommandRequest : IStreamFollowerUserCommandRequest,
    IRequest<HttpResult>,
    ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public Guid UserId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public StreamFollowerUserCreateCommandRequest()
    {
        AuthorizationFunctions = [StreamFollowerUserAuthorizationRules.CanUserFollowStreamer];
    }

    public StreamFollowerUserCreateCommandRequest(Guid streamerId, Guid userId) : this()
    {
        StreamerId = streamerId;
        UserId = userId;
    }
}

public sealed class
    StreamFollowerUserCreateCommandHandler : IRequestHandler<StreamFollowerUserCreateCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StreamFollowerUserCreateCommandHandler(IEfRepository efRepository, IHttpContextAccessor httpContextAccessor)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpResult> Handle(StreamFollowerUserCreateCommandRequest request,
        CancellationToken cancellationToken)
    {
        var streamFollowerUser = StreamFollowerUser.Create(request.StreamerId, request.UserId);

        _efRepository.StreamFollowerUsers.Add(streamFollowerUser);

        var result = await _efRepository.SaveChangesAsync(cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status201Created)
            : StreamFollowerUserErrors.FailedToFollowStreamer;
    }
}
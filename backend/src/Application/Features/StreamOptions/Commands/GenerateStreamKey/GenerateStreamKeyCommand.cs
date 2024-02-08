using Application.Common.Extensions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;
using Application.Features.Streams.Services;

namespace Application.Features.StreamOptions.Commands.Update;

public readonly record struct GenerateStreamKeyCommandRequest
    : IStreamOptionCommandRequest, IRequest<HttpResult>, ISecuredRequest
{
    public Guid StreamerId { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public GenerateStreamKeyCommandRequest()
    {
        AuthorizationFunctions =
            [StreamOptionAuthorizationRules.UserMustBeStreamer];
    }

    public GenerateStreamKeyCommandRequest(Guid streamerId, bool chatDisabled, bool mustBeFollower,
        int chatDelaySecond) : this()
    {
        StreamerId = streamerId;
    }
}

public sealed class
    GenerateStreamKeyCommandRequestHandler : IRequestHandler<GenerateStreamKeyCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;

    private readonly IStreamService _streamService;

    public GenerateStreamKeyCommandRequestHandler(IEfRepository efRepository, IStreamService streamService)
    {
        _efRepository = efRepository;
        _streamService = streamService;
    }

    public async Task<HttpResult> Handle(GenerateStreamKeyCommandRequest request, CancellationToken cancellationToken)
    {
        var user = await _efRepository
            .Users
            .SingleOrDefaultAsync(u => u.Id == request.StreamerId, cancellationToken: cancellationToken);

        var newStreamKey = _streamService.GenerateStreamKey(user);

        var result = await _efRepository.StreamOptions
            .Where(st => st.Id == request.StreamerId)
            .ExecuteUpdateAsync(
                streamer => streamer
                    .SetProperty(x => x.StreamKey, x => newStreamKey),
                cancellationToken:
                cancellationToken);

        return result > 0
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
    }
}
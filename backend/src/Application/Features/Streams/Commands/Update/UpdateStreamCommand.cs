using Application.Features.Streams.Abstractions;
using Application.Features.Streams.Rules;
using Application.Features.Streams.Services;

namespace Application.Features.Streams.Commands.Update;

public readonly record struct UpdateStreamCommandRequest : IStreamCommandRequest, IRequest<HttpResult>,
    IApiSecuredRequest
{
    public string StreamKey { get; init; }
    public AuthorizationFunctions AuthorizationFunctions { get; }

    public UpdateStreamCommandRequest()
    {
        AuthorizationFunctions = [StreamAuthorizationRules.RequesterMustHaveValidApiKey];
    }
}

public sealed class UpdateStreamCommandHandler : IRequestHandler<UpdateStreamCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamService _streamService;
    private readonly StreamBusinessRules _streamBusinessRules;

    public UpdateStreamCommandHandler(IEfRepository efRepository, IStreamService streamService,
        StreamBusinessRules streamBusinessRules)
    {
        _efRepository = efRepository;
        _streamService = streamService;
        _streamBusinessRules = streamBusinessRules;
    }

    public async Task<HttpResult> Handle(UpdateStreamCommandRequest request, CancellationToken cancellationToken)
    {
        var streamerId = _streamService.GetUserIdFromStreamKey(request.StreamKey);

        var liveStreams = await _streamService.GetLiveStreamsAsync();

        var streamLiveResult = _streamBusinessRules.IsStreamLive(liveStreams, streamerId);

        if (streamLiveResult.IsFailure)
        {
            return streamLiveResult.Error;
        }

        var liveStream = streamLiveResult.Value;

        var streamHasEnded = await _streamService.EndStreamAsync(liveStream, cancellationToken);

        return streamHasEnded
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamErrors.FailedToEndStream;
    }
}
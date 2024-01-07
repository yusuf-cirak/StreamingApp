using Application.Features.Streams.Abstractions;
using Application.Features.Streams.Rules;
using Application.Features.Streams.Services;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Commands.Create;

public readonly record struct CreateStreamCommandRequest : IStreamCommandRequest, IRequest<HttpResult<string>>,
    IApiSecuredRequest
{
    public string StreamKey { get; init; }

    public AuthorizationFunctions AuthorizationFunctions { get; }

    public CreateStreamCommandRequest()
    {
        // TODO: Add authorization with API key and Stream Key
        AuthorizationFunctions = [StreamAuthorizationRules.RequesterMustHaveValidApiKey];
    }
}

public sealed class CreateStreamCommandHandler : IRequestHandler<CreateStreamCommandRequest, HttpResult<string>>
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamService _streamService;

    public CreateStreamCommandHandler(IEfRepository efRepository, IStreamService streamService)
    {
        _efRepository = efRepository;
        _streamService = streamService;
    }

    public async Task<HttpResult<string>> Handle(CreateStreamCommandRequest request, CancellationToken cancellationToken)
    {
        var streamerIdResult = _streamService.GetUserIdFromStreamKey(request.StreamKey);
        
        if (streamerIdResult.IsFailure)
        {
            return streamerIdResult.Error;
        }
        
        var streamerId = streamerIdResult.Value;

        var streamerResult = await _streamService.StreamerExistsAsync(streamerId, cancellationToken);

        if (streamerResult.IsFailure)
        {
            return streamerResult.Error;
        }

        var isStreamerLiveResult = await _streamService.IsStreamerLiveAsync(streamerId, cancellationToken);

        if (isStreamerLiveResult.IsFailure)
        {
            return isStreamerLiveResult.Error;
        }

        var stream = Stream.Create(streamerId);

        var streamer = streamerResult.Value;

        var isStreamStarted = await _streamService.StartNewStreamAsync(streamer, stream, cancellationToken);

        //TODO: Add event to notify users that stream has started

        return isStreamStarted
            ? HttpResult<string>.Success(streamer.User.Username)
            : StreamErrors.FailedToCreateStream;
    }
}
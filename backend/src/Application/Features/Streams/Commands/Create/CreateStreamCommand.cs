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
        AuthorizationFunctions = [StreamAuthorizationRules.RequesterMustHaveValidApiKey];
    }
}

public sealed class CreateStreamCommandHandler : IRequestHandler<CreateStreamCommandRequest, HttpResult<string>>
{
    private readonly IStreamService _streamService;

    public CreateStreamCommandHandler(IStreamService streamService)
    {
        _streamService = streamService;
    }

    public async Task<HttpResult<string>> Handle(CreateStreamCommandRequest request,
        CancellationToken cancellationToken)
    {
        var streamOptionResult = await _streamService.StreamerExistsAsync(request.StreamKey, cancellationToken);

        if (streamOptionResult.IsFailure)
        {
            return streamOptionResult.Error;
        }

        var streamOptions = streamOptionResult.Value;

        var streamer = streamOptions.Streamer;

        var isStreamerLiveResult =
            _streamService.IsStreamerLive(streamer, request.StreamKey);

        if (isStreamerLiveResult.IsFailure)
        {
            return isStreamerLiveResult.Error;
        }

        var stream = Stream.Create(streamer.Id);

        var isStreamStarted = await _streamService.StartNewStreamAsync(streamOptions, stream, cancellationToken);

        if (!isStreamStarted)
        {
            return StreamErrors.FailedToCreateStream;
        }

        return streamer.Username;
    }
}
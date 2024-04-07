using Application.Common.Mapping;
using Application.Features.Streams.Abstractions;
using Application.Features.Streams.Rules;
using Application.Features.Streams.Services;
using SignalR.Hubs.Stream.Server.Abstractions;
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
    private readonly IStreamHubServerService _hubServerService;

    public CreateStreamCommandHandler(IStreamService streamService, IStreamHubServerService hubServerService)
    {
        _streamService = streamService;
        _hubServerService = hubServerService;
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
            await _streamService.IsStreamerLiveAsync(streamer, request.StreamKey, cancellationToken);

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

        var streamDto = stream.ToDto(streamer.ToDto(), streamOptions.ToStreamOptionDto());

        _ = _hubServerService.OnStreamStartedAsync(streamDto);

        return streamer.Username;
    }
}
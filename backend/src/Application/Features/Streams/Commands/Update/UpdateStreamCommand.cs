using Application.Features.Streams.Abstractions;
using Application.Features.Streams.Rules;
using Application.Features.Streams.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

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
    private readonly IStreamService _streamService;
    private readonly IStreamHubServerService _hubServerService;

    public UpdateStreamCommandHandler(IStreamService streamService, IStreamHubServerService hubServerService)
    {
        _streamService = streamService;
        _hubServerService = hubServerService;
    }

    public async Task<HttpResult> Handle(UpdateStreamCommandRequest request, CancellationToken cancellationToken)
    {
        var streamLiveResult = await _streamService.GetLiveStreamerByKeyAsync(request.StreamKey);

        if (streamLiveResult.IsFailure)
        {
            return streamLiveResult.Error;
        }

        var liveStream = streamLiveResult.Value;

        var streamHasEnded = await _streamService.EndStreamAsync(liveStream, cancellationToken);

        await _hubServerService.OnStreamEndAsync(liveStream.User.Username);

        return streamHasEnded
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamErrors.FailedToEndStream;
    }
}
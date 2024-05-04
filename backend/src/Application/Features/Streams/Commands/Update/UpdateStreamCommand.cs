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
        var streamLiveResult = _streamService.GetLiveStreamerByKeyFromCache(request.StreamKey, cancellationToken);

        if (streamLiveResult.IsFailure)
        {
            return streamLiveResult.Error;
        }

        var (streamDto, index) = streamLiveResult.Value;

        var streamHasEnded = await _streamService.EndStreamAsync(index, cancellationToken);

        _ = _hubServerService.OnStreamEndAsync(streamDto.User.Username);

        return streamHasEnded
            ? HttpResult.Success(StatusCodes.Status204NoContent)
            : StreamErrors.FailedToEndStream;
    }
}
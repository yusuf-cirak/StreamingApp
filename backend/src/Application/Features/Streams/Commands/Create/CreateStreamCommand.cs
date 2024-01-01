using Application.Features.Streams.Abstractions;
using Application.Features.Streams.Rules;
using Application.Features.Streams.Services;
using Stream = Domain.Entities.Stream;

namespace Application.Features.Streams.Commands.Create;

public readonly record struct CreateStreamCommandRequest : IStreamCommandRequest, IRequest<HttpResult<Guid>>,
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

public sealed class CreateStreamCommandHandler : IRequestHandler<CreateStreamCommandRequest, HttpResult<Guid>>
{
    private readonly IEfRepository _efRepository;
    private readonly IStreamService _streamService;

    public CreateStreamCommandHandler(IEfRepository efRepository, IStreamService streamService)
    {
        _efRepository = efRepository;
        _streamService = streamService;
    }

    public async Task<HttpResult<Guid>> Handle(CreateStreamCommandRequest request, CancellationToken cancellationToken)
    {
        var streamerId = _streamService.GetUserIdFromStreamKey(request.StreamKey);

        bool streamerExists = await _streamService.StreamerExistsAsync(streamerId, cancellationToken);

        if (!streamerExists)
        {
            return StreamErrors.StreamerNotExists;
        }

        var stream = Stream.Create(streamerId);

        var streamStartResult = await _streamService.StartNewStreamAsync(stream, cancellationToken);

        return streamStartResult > 0
            ? HttpResult<Guid>.Success(stream.Id, StatusCodes.Status201Created)
            : StreamErrors.FailedToCreateStream;
    }
}
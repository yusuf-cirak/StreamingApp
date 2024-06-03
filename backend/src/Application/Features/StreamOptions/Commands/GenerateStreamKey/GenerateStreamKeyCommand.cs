using Application.Common.Permissions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.Streams.Services;

namespace Application.Features.StreamOptions.Commands.Update;

public record struct GenerateStreamKeyCommandRequest
    : IStreamOptionRequest, IRequest<HttpResult<string>>, ISecuredRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;

            PermissionRequirements = PermissionRequirements.Create()
                .WithRoles(RequiredClaim.Create(RoleConstants.Streamer, StreamErrors.UserIsNotStreamer));
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class
    GenerateStreamKeyCommandRequestHandler : IRequestHandler<GenerateStreamKeyCommandRequest, HttpResult<string>>
{
    private readonly IEfRepository _efRepository;

    private readonly IStreamService _streamService;

    public GenerateStreamKeyCommandRequestHandler(IEfRepository efRepository, IStreamService streamService)
    {
        _efRepository = efRepository;
        _streamService = streamService;
    }

    public async Task<HttpResult<string>> Handle(GenerateStreamKeyCommandRequest request,
        CancellationToken cancellationToken)
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
            ? newStreamKey
            : StreamOptionErrors.CannotBeUpdated;
    }
}
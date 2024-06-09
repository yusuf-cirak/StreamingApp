using Application.Common.Permissions;
using Application.Features.StreamOptions.Abstractions;

namespace Application.Features.StreamOptions.Queries.GetStreamKey;

public record struct GetStreamKeyQueryRequest : IRequest<HttpResult<string>>, IPermissionRequest, IStreamOptionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;

            PermissionRequirements = PermissionRequirementConstants.WithNameIdentifier(value.ToString());
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }


    public GetStreamKeyQueryRequest(Guid streamerId)
    {
        StreamerId = streamerId;
    }
}

public sealed class GetStreamKeyQueryHandler : IRequestHandler<GetStreamKeyQueryRequest, HttpResult<string>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamKeyQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<string>> Handle(GetStreamKeyQueryRequest request, CancellationToken cancellationToken)
    {
        return await _efRepository
            .StreamOptions
            .Where(r => r.Id == request.StreamerId)
            .Select(r => r.StreamKey)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
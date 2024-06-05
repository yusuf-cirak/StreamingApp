using Application.Common.Mapping;
using Application.Common.Permissions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetChatSettings;

public record struct GetStreamChatSettingsQueryRequest : IStreamOptionRequest,
    IRequest<HttpResult<GetStreamChatSettingsDto>>,
    IPermissionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;

            this.PermissionRequirements = PermissionRequirements.Create()
                .WithRequiredValue(_streamerId.ToString())
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
                .WithOperationClaims(RequiredClaim.Create(OperationClaimConstants.Stream.Read.ChatOptions,
                    StreamErrors.UserIsNotModeratorOfStream))
                .WithNameIdentifierClaim();
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }

    public GetStreamChatSettingsQueryRequest(Guid streamerId)
    {
        StreamerId = streamerId;
    }
}

public sealed class
    GetStreamChatSettingsQueryHandler : IRequestHandler<GetStreamChatSettingsQueryRequest,
    HttpResult<GetStreamChatSettingsDto>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamChatSettingsQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetStreamChatSettingsDto>> Handle(GetStreamChatSettingsQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _efRepository
            .StreamOptions
            .Where(so => so.Id == request.StreamerId)
            .Select(so => so.ToStreamChatSettingsDto())
            .SingleOrDefaultAsync(cancellationToken);
    }
}
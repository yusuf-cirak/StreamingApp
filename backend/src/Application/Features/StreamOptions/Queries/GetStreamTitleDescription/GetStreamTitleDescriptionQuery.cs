using Application.Common.Mapping;
using Application.Common.Permissions;
using Application.Contracts.StreamOptions;
using Application.Features.StreamOptions.Rules;

namespace Application.Features.StreamOptions.Queries.GetStreamTitleDescription;

public record struct
    GetStreamTitleDescriptionQueryRequest : IRequest<HttpResult<GetStreamTitleDescriptionDto>>, ISecuredRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;
            this.PermissionRequirements = PermissionRequirements.Create()
                .WithRequiredValue(value.ToString())
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
                .WithOperationClaims(
                    RequiredClaim.Create(OperationClaimConstants.Stream.Read.TitleDescription,
                        StreamErrors.UserIsNotModeratorOfStream),
                    RequiredClaim.Create(OperationClaimConstants.Stream.Write.TitleDescription,
                        StreamErrors.UserIsNotModeratorOfStream));
        }
    }

    public PermissionRequirements PermissionRequirements { get; private set; }


    public GetStreamTitleDescriptionQueryRequest(Guid streamerId)
    {
        StreamerId = streamerId;
    }
}

public sealed class GetStreamTitleDescriptionQueryHandler : IRequestHandler<GetStreamTitleDescriptionQueryRequest,
    HttpResult<GetStreamTitleDescriptionDto>>
{
    private readonly IEfRepository _efRepository;

    public GetStreamTitleDescriptionQueryHandler(IEfRepository efRepository)
    {
        _efRepository = efRepository;
    }

    public async Task<HttpResult<GetStreamTitleDescriptionDto>> Handle(GetStreamTitleDescriptionQueryRequest request,
        CancellationToken cancellationToken)
    {
        return await _efRepository
            .StreamOptions
            .Where(so => so.Id == request.StreamerId)
            .Select(so => so.ToStreamTitleDescriptionDto())
            .SingleOrDefaultAsync(cancellationToken);
    }
}
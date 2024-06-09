using Application.Common.Permissions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Services;

namespace Application.Features.StreamOptions.Commands.Update;

public record struct UpdateStreamTitleDescriptionCommandRequest
    : IStreamOptionRequest, IRequest<HttpResult>, IPermissionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;
            this.PermissionRequirements = PermissionRequirements
                .Create()
                .WithRequiredValue(_streamerId.ToString())
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
                .WithOperationClaims(RequiredClaim.Create(OperationClaimConstants.Stream.Write.TitleDescription,
                    StreamErrors.UserIsNotModeratorOfStream))
                .WithNameIdentifierClaim();
        }
    }

    public IFormFile? Thumbnail { get; init; }

    public string ThumbnailUrl { get; init; }

    public string StreamTitle { get; init; }

    public string StreamDescription { get; init; }

    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class
    UpdateStreamTitleDescriptionCommandHandler : IRequestHandler<UpdateStreamTitleDescriptionCommandRequest, HttpResult>
{
    private readonly IEfRepository _efRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStreamOptionService _streamOptionService;

    public UpdateStreamTitleDescriptionCommandHandler(IEfRepository efRepository,
        IHttpContextAccessor httpContextAccessor, IStreamOptionService streamOptionService)
    {
        _efRepository = efRepository;
        _httpContextAccessor = httpContextAccessor;
        _streamOptionService = streamOptionService;
    }

    public async Task<HttpResult> Handle(UpdateStreamTitleDescriptionCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.GetUserId());

        var streamOptionsResult = await _streamOptionService.GetStreamOptionAsync(userId, cancellationToken);

        if (streamOptionsResult.IsFailure)
        {
            return streamOptionsResult.Error;
        }

        var streamOptions = streamOptionsResult.Value;

        string thumbnailUrl = await
            _streamOptionService.UploadStreamThumbnailImageAsync(streamOptions, request.Thumbnail,
                request.ThumbnailUrl);

        streamOptions.Update(request.StreamTitle, request.StreamDescription, thumbnailUrl);

        await _efRepository.SaveChangesAsync(cancellationToken);

        _ = _streamOptionService.UpdateStreamOptionCacheAndSendNotificationAsync(streamOptions, cancellationToken);

        return HttpResult.Success(StatusCodes.Status204NoContent);
    }
}
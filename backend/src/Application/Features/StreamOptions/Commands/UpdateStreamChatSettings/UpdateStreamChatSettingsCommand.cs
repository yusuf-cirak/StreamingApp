using Application.Common.Permissions;
using Application.Features.StreamOptions.Abstractions;
using Application.Features.StreamOptions.Services;

namespace Application.Features.StreamOptions.Commands.Update;

public record struct UpdateStreamChatSettingsCommandRequest()
    : IStreamOptionRequest, IRequest<HttpResult>, IPermissionRequest
{
    private Guid _streamerId;

    public Guid StreamerId
    {
        get => _streamerId;
        set
        {
            _streamerId = value;

            PermissionRequirements = PermissionRequirements
                .Create()
                .WithRequiredValue(value.ToString())
                .WithRoles(PermissionHelper.AllStreamRoles().ToArray())
                .WithOperationClaims(RequiredClaim.Create(OperationClaimConstants.Stream.Write.ChatOptions,
                    StreamErrors.UserIsNotModeratorOfStream))
                .WithNameIdentifierClaim();
        }
    }

    public bool ChatDisabled { get; init; } = false;

    public bool MustBeFollower { get; init; } = false;

    public int ChatDelaySecond { get; init; } = 0;
    public PermissionRequirements PermissionRequirements { get; private set; }
}

public sealed class
    UpdateChatSettingsCommandHandler(
        IEfRepository efRepository,
        IStreamOptionService streamOptionService)
    : IRequestHandler<UpdateStreamChatSettingsCommandRequest, HttpResult>
{
    public async Task<HttpResult> Handle(UpdateStreamChatSettingsCommandRequest request,
        CancellationToken cancellationToken)
    {
        var streamOptions = await
            efRepository
                .StreamOptions
                .Include(so => so.Streamer)
                .ThenInclude(user => user.Streams)
                .AsTracking()
                .SingleOrDefaultAsync(so => so.Id == request.StreamerId,
                    cancellationToken: cancellationToken);

        streamOptions.Update(request.MustBeFollower,
            request.ChatDisabled, request.ChatDelaySecond);


        var result = await efRepository.SaveChangesAsync(cancellationToken);

        if (result is 0)
        {
            return HttpResult.Failure(StreamOptionErrors.CannotBeUpdated);
        }

        _ = Task.Run(() => streamOptionService.UpdateStreamOptionCacheAndSendNotificationAsync(streamOptions,
            cancellationToken: cancellationToken), cancellationToken);

        return HttpResult.Success(StatusCodes.Status204NoContent);
    }
}
using Application.Common.Services;
using Application.Features.Users.Services;
using SignalR.Hubs.Stream.Server.Abstractions;

namespace Application.Features.Users.Commands.DeleteStreamModerator;

public sealed record DeleteStreamModeratorsCommandRequest(List<Guid> UserIds)
    : IRequest<HttpResult<bool>>, ISecuredRequest;

public sealed class DeleteStreamModeratorCommandHandler(
    IUserService userService,
    ICurrentUserService currentUserService,
    IEfRepository efRepository,
    IStreamHubServerService streamHubServerService,
    IUserBlacklistManager blacklistManager
)
    : IRequestHandler<DeleteStreamModeratorsCommandRequest, HttpResult<bool>>
{
    public async Task<HttpResult<bool>> Handle(DeleteStreamModeratorsCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var usersResult = await userService.GetUsersByIdsAsync(request.UserIds, cancellationToken);

        if (usersResult.IsFailure)
        {
            return usersResult.Error;
        }

        var users = usersResult.Value;

        userService.DeleteStreamModerators(users, userId.ToString());

        var userIds = request.UserIds.Select(id => id.ToString()).ToList();

        Task[] tasks =
        [
            efRepository.SaveChangesAsync(cancellationToken),
            blacklistManager.AddUsersToBlacklistAsync(userIds)
        ];

        await Task.WhenAll(tasks);

        _ = Task.Run(() => streamHubServerService.OnUpsertModeratorsAsync(userIds), cancellationToken);

        return true;
    }
}
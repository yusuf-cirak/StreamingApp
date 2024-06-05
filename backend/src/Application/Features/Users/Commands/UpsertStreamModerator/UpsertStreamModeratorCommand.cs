using Application.Common.Services;
using Application.Features.Users.Services;

namespace Application.Features.Users.Commands.UpsertStreamModerator;

public sealed record UpsertStreamModeratorsCommandRequest(
    List<Guid> UserIds,
    List<Guid> RoleIds,
    List<Guid> OperationClaimIds) : IRequest<HttpResult<bool>>, ISecuredRequest;

public sealed class UserCreateStreamPermissionCommandRequestHandler(
    ICurrentUserService currentUserService,
    IUserService userService,
    IEfRepository efRepository)
    : IRequestHandler<UpsertStreamModeratorsCommandRequest, HttpResult<bool>>
{
    public async Task<HttpResult<bool>> Handle(UpsertStreamModeratorsCommandRequest request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        var usersResult = await userService.GetUsersByIdsAsync(request.UserIds, cancellationToken);

        if (usersResult.IsFailure)
        {
            return usersResult.Error;
        }

        var users = usersResult.Value;

        userService.UpdateUsersWithRolesAndOperationClaims(users, request.RoleIds, request.OperationClaimIds,
            userId.ToString());


        await efRepository.SaveChangesAsync(cancellationToken);

        // todo: send notification users to refresh their tokens

        return true;
    }
}
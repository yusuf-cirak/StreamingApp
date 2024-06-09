namespace Application.Common.Permissions;

public static class PermissionHelper
{
    public static IEnumerable<RequiredClaim> AllStreamRoles()
    {
        return
        [
            RequiredClaim.Create(RoleConstants.StreamModerator, StreamErrors.UserIsNotModeratorOfStream),
            RequiredClaim.Create(RoleConstants.StreamSuperModerator, StreamErrors.UserIsNotModeratorOfStream),
        ];
    }
}
namespace Application.Common.Permissions;

public static class PermissionRequirementConstants
{
    public static PermissionRequirements WithAdminRole() => PermissionRequirements.Create()
        .WithRoles(RequiredClaim.Create(RoleConstants.Admin, Error.None));

    public static PermissionRequirements WithNameIdentifier(string streamerId) => PermissionRequirements.Create()
        .WithRequiredValue(streamerId)
        .WithClaims(RequiredClaim.Create(ClaimTypes.NameIdentifier, StreamErrors.UserIsNotStreamer));


    public static PermissionRequirements WithNameIdentifierClaim(this PermissionRequirements permissionRequirements) =>
        permissionRequirements
            .WithClaims(RequiredClaim.Create(ClaimTypes.NameIdentifier, StreamErrors.UserIsNotStreamer));
}
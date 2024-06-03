namespace Application.Common.Permissions;

public static class PermissionRequirementConstants
{
    public static PermissionRequirements WithAdminRole() => PermissionRequirements.Create()
        .WithRoles(RequiredClaim.Create(RoleConstants.SystemAdmin, Error.None));
}
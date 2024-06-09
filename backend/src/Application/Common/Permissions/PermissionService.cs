using Application.Common.Extensions;

namespace Application.Common.Permissions;

public interface IPermissionService
{
    Result Authorize(PermissionRequirements permissionRequirements, ClaimsPrincipal user);
}

public sealed class PermissionService : IPermissionService
{
    public Result Authorize(PermissionRequirements permissionRequirements, ClaimsPrincipal user)
    {
        var roles = permissionRequirements.Roles;
        List<Result> authorizationResults = new();

        if (permissionRequirements.Roles.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByRole(permissionRequirements, user));
        }

        if (permissionRequirements.OperationClaims.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByOperationClaim(permissionRequirements, user));
        }

        if (permissionRequirements.Claims.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByClaim(permissionRequirements, user));
        }


        if (authorizationResults.Count() is 0)
        {
            return Result.Success();
        }

        var failureIndex = authorizationResults.FindIndex(ar => ar.IsFailure);
        var successExists = authorizationResults.Exists(ar => ar.IsSuccess);


        if (permissionRequirements.MatchMode is MatchMode.All)
        {
            return failureIndex is not -1 ? Result.Failure(authorizationResults[failureIndex].Error) : Result.Success();
        }


        return successExists || failureIndex is -1
            ? Result.Success()
            : Result.Failure(authorizationResults[failureIndex].Error);
    }


    public Result AuthorizeByRole(PermissionRequirements permissionRequirements, ClaimsPrincipal user)
    {
        var userRoles = user.Claims.GetRoles();

        IEnumerable<Error> roleErrors = new List<Error>();

        if (permissionRequirements.MatchMode is MatchMode.Any)
        {
            bool anyRequiredRoleFound = permissionRequirements.Roles.Any(requiredRole =>
                userRoles.Any(userRole =>
                    userRole.Name.Equals(requiredRole.Name, StringComparison.OrdinalIgnoreCase) &&
                    userRole.Value.Equals(permissionRequirements.RequiredValue, StringComparison.OrdinalIgnoreCase))
            );

            if (!anyRequiredRoleFound)
            {
                roleErrors = permissionRequirements.Roles
                    .Where(requiredRole => !userRoles.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredRole.Name, StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(permissionRequirements.RequiredValue,
                            StringComparison.OrdinalIgnoreCase)))
                    .Select(requiredOperationClaim => requiredOperationClaim.Error);
            }
        }
        else
        {
            roleErrors = permissionRequirements.Roles
                .Where(requiredRole => userRoles.Any(userRole =>
                    userRole.Name != requiredRole.Name && userRole.Value != permissionRequirements.RequiredValue))
                .Select(requiredClaim => requiredClaim.Error);
        }

        var errors = roleErrors.ToList();
        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }

    public Result AuthorizeByOperationClaim(PermissionRequirements permissionRequirements, ClaimsPrincipal user)
    {
        var userOperationClaims = user.Claims.GetOperationClaims();

        IEnumerable<Error> operationClaimErrors = [];

        if (permissionRequirements.MatchMode == MatchMode.Any)
        {
            bool anyRequiredOperationClaimFound = permissionRequirements.OperationClaims.Any(requiredOperationClaim =>
                userOperationClaims.Any(userOperationClaim =>
                    userOperationClaim.Name.Equals(requiredOperationClaim.Name, StringComparison.OrdinalIgnoreCase) &&
                    userOperationClaim.Value.Equals(permissionRequirements.RequiredValue,
                        StringComparison.OrdinalIgnoreCase))
            );

            if (!anyRequiredOperationClaimFound)
            {
                operationClaimErrors = permissionRequirements.OperationClaims
                    .Where(requiredOperationClaim => !userOperationClaims.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredOperationClaim.Name,
                            StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(permissionRequirements.RequiredValue,
                            StringComparison.OrdinalIgnoreCase)))
                    .Select(requiredOperationClaim => requiredOperationClaim.Error);
            }
        }
        else
        {
            // If any required operation claim is not found in userOperationClaims, return error.
            operationClaimErrors = permissionRequirements.OperationClaims
                .Where(requiredOperationClaim =>
                    !userOperationClaims.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredOperationClaim.Name,
                            StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(permissionRequirements.RequiredValue,
                            StringComparison.OrdinalIgnoreCase))
                )
                .Select(requiredOperationClaim => requiredOperationClaim.Error);
        }

        var errors = operationClaimErrors.ToList();
        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }


    public Result AuthorizeByClaim(PermissionRequirements permissionRequirements, ClaimsPrincipal user)
    {
        var userClaims = user.Claims;

        IEnumerable<Error> errors = permissionRequirements.Claims
            .Where(requiredClaim => !userClaims.Any(userClaim =>
                userClaim.Type == requiredClaim.Name && userClaim.Value == permissionRequirements.RequiredValue))
            .Select(requiredClaim => requiredClaim.Error)
            .ToList();

        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }
}
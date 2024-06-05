using Application.Common.Extensions;

namespace Application.Common.Permissions;

public sealed class PermissionRequirements
{
    private IEnumerable<RequiredClaim> Roles { get; set; } = [];
    private IEnumerable<RequiredClaim> OperationClaims { get; set; } = [];

    private IEnumerable<RequiredClaim> Claims { get; set; } = [];

    private string RequiredValue { get; set; }

    private MatchMode MatchMode { get; set; } = MatchMode.Any;


    public static PermissionRequirements Create() => new();

    public static PermissionRequirements Create(Action<PermissionRequirements> userPermissionHandler)
    {
        var newUserPermissionHandler = new PermissionRequirements();

        userPermissionHandler(newUserPermissionHandler);

        return newUserPermissionHandler;
    }


    public PermissionRequirements WithRoles(Action<IEnumerable<RequiredClaim>> requiredClaims)
    {
        requiredClaims(this.Roles);

        return this;
    }

    public PermissionRequirements WithRoles(params RequiredClaim[] requiredClaims)
    {
        this.Roles = requiredClaims;

        return this;
    }

    public PermissionRequirements WithOperationClaims(Action<IEnumerable<RequiredClaim>> requiredClaims)
    {
        requiredClaims(this.OperationClaims);

        return this;
    }

    public PermissionRequirements WithOperationClaims(params RequiredClaim[] requiredClaims)
    {
        this.OperationClaims = requiredClaims;

        return this;
    }

    public PermissionRequirements WithClaims(params RequiredClaim[] requiredClaims)
    {
        this.Claims = requiredClaims;

        return this;
    }

    public PermissionRequirements WithRequiredValue(string requiredValue)
    {
        this.RequiredValue = requiredValue;

        return this;
    }

    public PermissionRequirements WithMatchMode(MatchMode matchMode)
    {
        this.MatchMode = matchMode;

        return this;
    }


    public Result Authorize(ClaimsPrincipal user)
    {
        List<Result> authorizationResults = new();

        if (this.Roles.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByRole(user));
        }

        if (this.OperationClaims.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByOperationClaim(user));
        }

        if (this.Claims.Count() is not 0)
        {
            authorizationResults.Add(this.AuthorizeByClaim(user));
        }


        if (authorizationResults.Count() is 0)
        {
            return Result.Success();
        }

        var failureIndex = authorizationResults.FindIndex(ar => ar.IsFailure);
        var successExists = authorizationResults.Exists(ar => ar.IsSuccess);


        if (this.MatchMode is MatchMode.All)
        {
            return failureIndex is not -1 ? Result.Failure(authorizationResults[failureIndex].Error) : Result.Success();
        }


        return successExists || failureIndex is -1
            ? Result.Success()
            : Result.Failure(authorizationResults[failureIndex].Error);
    }


    public Result AuthorizeByRole(ClaimsPrincipal user)
    {
        var userRoles = user.Claims.GetRoles();

        IEnumerable<Error> roleErrors = new List<Error>();

        if (this.MatchMode is MatchMode.Any)
        {
            bool anyRequiredRoleFound = this.Roles.Any(requiredRole =>
                userRoles.Any(userRole =>
                    userRole.Name.Equals(requiredRole.Name, StringComparison.OrdinalIgnoreCase) &&
                    userRole.Value.Equals(this.RequiredValue, StringComparison.OrdinalIgnoreCase))
            );

            if (!anyRequiredRoleFound)
            {
                roleErrors = this.Roles
                    .Where(requiredRole => !userRoles.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredRole.Name, StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(this.RequiredValue, StringComparison.OrdinalIgnoreCase)))
                    .Select(requiredOperationClaim => requiredOperationClaim.Error);
            }
        }
        else
        {
            roleErrors = this.Roles
                .Where(requiredRole => userRoles.Any(userRole =>
                    userRole.Name != requiredRole.Name && userRole.Value != this.RequiredValue))
                .Select(requiredClaim => requiredClaim.Error);
        }

        var errors = roleErrors.ToList();
        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }

    public Result AuthorizeByOperationClaim(ClaimsPrincipal user)
    {
        var userOperationClaims = user.Claims.GetOperationClaims();

        IEnumerable<Error> operationClaimErrors = [];

        if (this.MatchMode == MatchMode.Any)
        {
            bool anyRequiredOperationClaimFound = this.OperationClaims.Any(requiredOperationClaim =>
                userOperationClaims.Any(userOperationClaim =>
                    userOperationClaim.Name.Equals(requiredOperationClaim.Name, StringComparison.OrdinalIgnoreCase) &&
                    userOperationClaim.Value.Equals(this.RequiredValue, StringComparison.OrdinalIgnoreCase))
            );

            if (!anyRequiredOperationClaimFound)
            {
                operationClaimErrors = this.OperationClaims
                    .Where(requiredOperationClaim => !userOperationClaims.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredOperationClaim.Name,
                            StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(this.RequiredValue, StringComparison.OrdinalIgnoreCase)))
                    .Select(requiredOperationClaim => requiredOperationClaim.Error);
            }
        }
        else
        {
            // If any required operation claim is not found in userOperationClaims, return error.
            operationClaimErrors = this.OperationClaims
                .Where(requiredOperationClaim =>
                    !userOperationClaims.Any(userOperationClaim =>
                        userOperationClaim.Name.Equals(requiredOperationClaim.Name,
                            StringComparison.OrdinalIgnoreCase) &&
                        userOperationClaim.Value.Equals(this.RequiredValue,
                            StringComparison.OrdinalIgnoreCase))
                )
                .Select(requiredOperationClaim => requiredOperationClaim.Error);
        }

        var errors = operationClaimErrors.ToList();
        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }


    public Result AuthorizeByClaim(ClaimsPrincipal user)
    {
        var userClaims = user.Claims;

        IEnumerable<Error> errors = this.Claims
            .Where(requiredClaim => !userClaims.Any(userClaim =>
                userClaim.Type == requiredClaim.Name && userClaim.Value == this.RequiredValue))
            .Select(requiredClaim => requiredClaim.Error)
            .ToList();

        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }
}
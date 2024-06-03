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
        var roleAuthorizeResult = this.AuthorizeByRole(user);

        if (!roleAuthorizeResult.IsFailure) return Result.Success();


        var operationClaimAuthorizeResult = this.AuthorizeByOperationClaim(user);

        return operationClaimAuthorizeResult.IsFailure
            ? Result.Failure(operationClaimAuthorizeResult.Error)
            : Result.Success();
    }


    public Result AuthorizeByRole(ClaimsPrincipal user)
    {
        var userRoles = user.Claims.GetRoles();

        IEnumerable<Error> roleErrors;

        if (this.MatchMode is MatchMode.Any)
        {
            roleErrors = this.Roles
                .Where(requiredRole => userRoles.All(userRole =>
                    userRole.Name != requiredRole.Name && userRole.Value != this.RequiredValue))
                .Select(requiredClaim => requiredClaim.Error);
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

        IEnumerable<Error> operationClaimErrors;

        if (this.MatchMode is MatchMode.Any)
        {
            operationClaimErrors = this.OperationClaims
                .Where(requiredOperationClaim => userOperationClaims.All(userOperationClaim =>
                    userOperationClaim.Name != requiredOperationClaim.Name &&
                    userOperationClaim.Value != this.RequiredValue))
                .Select(requiredOperationClaim => requiredOperationClaim.Error);
        }
        else
        {
            operationClaimErrors = this.OperationClaims
                .Where(requiredOperationClaim => userOperationClaims.Any(userOperationClaim =>
                    userOperationClaim.Name != requiredOperationClaim.Name &&
                    userOperationClaim.Value != this.RequiredValue))
                .Select(requiredOperationClaim => requiredOperationClaim.Error);
        }

        var errors = operationClaimErrors.ToList();
        return !errors.Any() ? Result.Success() : Result.Failure(errors.First());
    }
}
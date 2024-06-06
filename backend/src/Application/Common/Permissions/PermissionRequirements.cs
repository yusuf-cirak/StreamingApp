namespace Application.Common.Permissions;

public sealed class PermissionRequirements 
{
    internal IEnumerable<RequiredClaim> Roles { get; private set; } = [];
    internal IEnumerable<RequiredClaim> OperationClaims { get; private set; } = [];

    internal IEnumerable<RequiredClaim> Claims { get; private set; } = [];

    internal string RequiredValue { get; private set; }

    internal MatchMode MatchMode { get; private set; } = MatchMode.Any;


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
}
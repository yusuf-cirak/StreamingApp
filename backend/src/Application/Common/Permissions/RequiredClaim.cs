namespace Application.Common.Permissions;

public sealed record RequiredClaim
{
    public string Name { get; set; }
    public Error Error { get; set; } = Error.None;


    public RequiredClaim()
    {
    }

    public RequiredClaim(string name, Error error)
    {
        Name = name;
        this.Error = error;
    }


    public static RequiredClaim Create(Action<RequiredClaim> requiredClaim)
    {
        var newRequiredClaim = new RequiredClaim();
        requiredClaim(newRequiredClaim);

        return newRequiredClaim;
    }

    public static RequiredClaim Create(string name, Error error)
        => new(name, error);


    public RequiredClaim WithName(string name)
    {
        this.Name = name;
        return this;
    }


    public RequiredClaim WithError(Error error)
    {
        this.Error = error;
        return this;
    }
}
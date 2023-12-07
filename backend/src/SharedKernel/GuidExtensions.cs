namespace SharedKernel;

public static class GuidExtensions
{
    public static Guid ToShortGuid(this Guid guid)
    {
        return Guid.Parse(guid.ToString().Replace("-","")[0..10]);
    }
}

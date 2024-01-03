namespace Application.Common.Constants;

public static class RedisConstant
{
    public static RedisKeyConstant Key { get; } = new();
}

public sealed class RedisKeyConstant
{
    public string LiveStreamers { get; } = "LiveStreams";

    internal protected RedisKeyConstant()
    {
        
    }
}
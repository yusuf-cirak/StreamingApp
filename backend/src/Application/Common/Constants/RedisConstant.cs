namespace Application.Common.Constants;
    public static class RedisConstant
    {
        public static readonly RedisKeyConstant Key = new();

        public sealed class RedisKeyConstant
        {
            public string LiveStreamers => "LiveStreams";

            internal RedisKeyConstant()
            {
            }
        }
    }

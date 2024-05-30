namespace Domain.Constants;

public static class OperationClaimConstants
{
    public static class Stream
    {
        public static class Read
        {
            public const string TitleDescription = "Stream::Read::TitleDescription";
            public const string ChatOptions = "Stream::Read::ChatOptions";
            public const string BlockFromChat = "Stream::Read::BlockFromChat";
            public const string DelayChat = "Stream::Read::DelayChat";

        }

        public static class Write
        {
            public const string TitleDescription = "Stream::Write::TitleDescription";
            public const string ChatOptions = "Stream::Write::ChatOptions";
            public const string BlockFromChat = "Stream::Write::BlockFromChat";
            public const string DelayChat = "Stream::Write::DelayChat";
        }
        
    }
}
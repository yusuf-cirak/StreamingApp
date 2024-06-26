﻿namespace SignalR.Constants;

public static class StreamHubConstant
{
    public const string AnonymousUser = "AnonymousUser";

    public static class Method
    {
        public const string OnStreamStartedAsync = "OnStreamStartedAsync";
        public const string OnStreamEndAsync = "OnStreamEndAsync";
        public const string OnStreamChatOptionsChangedAsync = "OnStreamChatOptionsChangedAsync";
        public const string OnStreamChatMessageSendAsync = "OnStreamChatMessageSendAsync";
        public const string OnBlockFromStreamAsync = "OnBlockFromStreamAsync";
        public const string OnUpsertModeratorAsync = "OnUpsertModeratorAsync";
    }
}
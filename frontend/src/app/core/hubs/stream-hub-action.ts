export enum StreamHubAction {
  OnJoinedStream = 'OnJoinedStreamAsync',
  OnLeavedStream = 'OnLeavedStreamAsync',
  OnStreamStarted = 'OnStreamStartedAsync',
  OnStreamEnd = 'OnStreamEndAsync',
  OnStreamChatOptionsChanged = 'OnStreamChatOptionsChangedAsync',
  OnStreamChatMessageSend = 'OnStreamChatMessageSendAsync',
  OnBlockFromStream = 'OnBlockFromStreamAsync',
  OnUpsertModerator = 'OnUpsertModeratorAsync',
}

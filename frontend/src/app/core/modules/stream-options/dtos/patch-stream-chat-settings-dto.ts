export interface PatchStreamChatSettingsDto {
  streamerId: string;
  chatDisabled: boolean;
  mustBeFollower: boolean;
  chatDelaySecond: number;
}

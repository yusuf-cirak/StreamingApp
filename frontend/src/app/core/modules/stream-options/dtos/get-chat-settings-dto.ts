export type GetChatSettingsDto = {
  streamerId: string;
  chatDisabled: boolean;
  mustBeFollower: boolean;
  chatDelaySecond: number;
};

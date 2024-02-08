export interface User {
  id: string;
  username: string;
  profileImageUrl: string;
}

export interface StreamOption {
  title: string;
  description: string;
  chatDisabled: boolean;
  chatDelaySecond: number;
}

export interface LiveStream {
  id: string;
  startedAt: string;
  user: User;
  streamOption: StreamOption;
  streamKey: string;
}

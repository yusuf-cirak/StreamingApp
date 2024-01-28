import { User } from '@streaming-app/core';
import { StreamOptions } from '../../../models/stream-options';

export interface LiveStream {
  id: string;
  startedAt: Date;
  user: User;
  options: StreamOptions;
}

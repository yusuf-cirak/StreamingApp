import { User } from '@streaming-app/core';
import { StreamOptions } from '../../../models/stream-options';

export interface LiveStreamDto {
  startedAt: Date;
  user: User;
  options: StreamOptions;
}

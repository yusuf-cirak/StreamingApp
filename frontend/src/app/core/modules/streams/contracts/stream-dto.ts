import { User } from '@streaming-app/core';
import { StreamOptions } from '../../../models/stream-options';

export interface StreamDto {
  startedAt?: Date;
  user: User;
  streamOption: StreamOptions;
}

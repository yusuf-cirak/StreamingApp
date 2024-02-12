import { StreamOptions } from './../../../models/stream-options';
import { User } from '../../../models';

export type StreamInfoDto = {
  startedAt: Date;
  streamOption: StreamOptions;
  user: User;
};

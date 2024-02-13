import { Error } from '../../../../shared/api/error';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';

export interface StreamState {
  stream?: LiveStreamDto;
  error?: Error;
}

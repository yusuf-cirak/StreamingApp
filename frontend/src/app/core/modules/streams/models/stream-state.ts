import { Error } from '../../../../shared/api/error';
import { StreamDto } from '../contracts/stream-dto';

export interface StreamState {
  stream: StreamDto | null;
  error: Error | null;
}

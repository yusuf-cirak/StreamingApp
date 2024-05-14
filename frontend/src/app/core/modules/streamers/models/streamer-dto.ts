import { User } from '../../../models';
import { StreamDto } from '../../streams/contracts/stream-dto';

export type StreamerDto = StreamDto | User;

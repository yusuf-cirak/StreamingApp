import { StreamDto } from '../../streams/contracts/stream-dto';
import { FollowingStreamerDto } from './following-stream-dto';

export type StreamerDto = StreamDto | FollowingStreamerDto;

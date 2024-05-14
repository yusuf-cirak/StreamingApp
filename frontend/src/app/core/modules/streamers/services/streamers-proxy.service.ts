import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { FollowingStreamerDto } from '../models/following-stream-dto';
import { StreamDto } from '../../streams/contracts/stream-dto';

@Injectable()
export class StreamersProxyService {
  private readonly httpClientService = inject(HttpClientService);

  getFollowing() {
    return this.httpClientService.get<FollowingStreamerDto[]>({
      controller: 'users',
      action: 'following-streamers',
    });
  }

  getLive() {
    return this.httpClientService.get<StreamDto[]>({
      controller: 'streams',
      action: 'live',
    });
  }
}

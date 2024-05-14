import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '@streaming-app/shared/services';
import { FollowingStreamerDto } from '../../recommended-streamers/models/following-stream-dto';
import { BlockedStreamerDto } from '../../recommended-streamers/models/blocked-streamer-dto';

@Injectable({
  providedIn: 'root',
})
export class UserProxyService {
  readonly httpClientService = inject(HttpClientService);

  getFollowingStreamers(): Observable<FollowingStreamerDto[]> {
    return this.httpClientService.get<FollowingStreamerDto[]>({
      controller: 'users',
      action: 'following-streamers',
    });
  }

  getBlockedStreamers(): Observable<BlockedStreamerDto[]> {
    return this.httpClientService.get<BlockedStreamerDto[]>({
      controller: 'users',
      action: 'blocked-streamers',
    });
  }
}

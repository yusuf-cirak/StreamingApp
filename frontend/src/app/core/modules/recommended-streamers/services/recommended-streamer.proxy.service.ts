import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { FollowingStreamerDto } from '../models/following-stream-dto';
import { HttpClientService } from '@streaming-app/shared/services';
import { LiveStreamDto } from '../models/live-stream-dto';

@Injectable({
  providedIn: 'root',
})
export class StreamerProxyService {
  readonly httpClientService = inject(HttpClientService);

  getLiveStreamers(): Observable<LiveStreamDto[]> {
    return this.httpClientService.get<LiveStreamDto[]>({
      controller: 'streamers',
      action: 'live',
    });
  }

  getFollowingStreamers(): Observable<FollowingStreamerDto[]> {
    return this.httpClientService.get<FollowingStreamerDto[]>({
      controller: 'streamers',
      action: 'following',
    });
  }
}

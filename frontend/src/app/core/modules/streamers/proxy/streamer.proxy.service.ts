import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Streamer } from '../models/streamer';
import { HttpClientService } from '@streaming-app/shared/services';
import { LiveStream } from '../models/live-stream';

@Injectable({
  providedIn: 'root',
})
export class StreamerProxyService {
  readonly httpClientService = inject(HttpClientService);

  getLiveStreamers(): Observable<LiveStream[]> {
    return this.httpClientService.get<LiveStream[]>({
      controller: 'streamers',
      action: 'live',
    });
  }

  getFollowingStreamers(): Observable<Streamer[]> {
    return this.httpClientService.get<Streamer[]>({
      controller: 'streamers',
      action: 'following',
    });
  }
}

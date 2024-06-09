import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { Observable } from 'rxjs';
import { StreamState } from '../models/stream-state';
import { StreamDto } from '../contracts/stream-dto';
import { BlockedStreamerDto } from '../../streamers/models/blocked-streamer-dto';

@Injectable({ providedIn: 'root' })
export class StreamProxyService {
  readonly httpClient = inject(HttpClientService);

  getLive() {
    return this.httpClient.get<StreamDto[]>({
      controller: 'streams',
      action: 'live',
    });
  }

  getInfo(streamerName: string): Observable<StreamState> {
    return this.httpClient.get<StreamState>({
      controller: 'streams',
      action: 'live',
      routeParams: [streamerName],
    });
  }

  getViewerCount(streamerName: string): Observable<number> {
    return this.httpClient.get<number>({
      controller: 'streams',
      action: 'viewer-count',
      routeParams: [streamerName],
    });
  }

  getFollowing() {
    return this.httpClient.get<StreamDto[]>({
      controller: 'streams',
      action: 'following',
    });
  }

  getBlocked() {
    return this.httpClient.get<BlockedStreamerDto[]>({
      controller: 'streams',
      action: 'blocked',
    });
  }

  getRecommended() {
    return this.httpClient.get<StreamDto[]>({
      controller: 'streams',
      action: 'recommended',
    });
  }
}

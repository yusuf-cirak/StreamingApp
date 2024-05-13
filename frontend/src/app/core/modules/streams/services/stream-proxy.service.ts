import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { Observable } from 'rxjs';
import { StreamState } from '../models/stream-state';

@Injectable({ providedIn: 'root' })
export class StreamProxyService {
  readonly httpClient = inject(HttpClientService);

  getStreamInfo(streamerName: string): Observable<StreamState> {
    return this.httpClient.get<StreamState>({
      controller: 'streams',
      action: 'live',
      routeParams: [streamerName],
    });
  }

  getStreamerViewerCount(streamerName: string): Observable<number> {
    return this.httpClient.get<number>({
      controller: 'streams',
      action: 'viewer-count',
      routeParams: [streamerName],
    });
  }
}

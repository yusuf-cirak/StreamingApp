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
    // map(
    //   (res) =>
    //     ({
    //       startedAt: res.startedAt,
    //       options: res.streamOption,
    //       user: res.user,
    //     } as StreamDto)
    // )
    // catchError((err) => throwError(err.error as Error))
  }
}

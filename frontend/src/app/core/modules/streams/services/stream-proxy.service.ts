import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { Observable, map } from 'rxjs';
import { StreamInfoDto } from '../contracts/stream-info-dto';

@Injectable({ providedIn: 'root' })
export class StreamProxyService {
  readonly httpClient = inject(HttpClientService);

  getStreamInfo(streamerName: string): Observable<LiveStreamDto> {
    return this.httpClient
      .get<StreamInfoDto>({
        controller: 'streams',
        action: 'live',
        routeParams: [streamerName],
      })
      .pipe(
        map(
          (res) =>
            ({
              startedAt: res.startedAt,
              options: res.streamOption,
              user: res.user,
            } as LiveStreamDto)
        )
      );
  }
}

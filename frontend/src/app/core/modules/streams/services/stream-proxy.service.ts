import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { Observable, catchError, map, of, throwError } from 'rxjs';
import { StreamInfoDto } from '../contracts/stream-info-dto';
import { Error } from '../../../../shared/api/error';

@Injectable({ providedIn: 'root' })
export class StreamProxyService {
  readonly httpClient = inject(HttpClientService);

  getStreamInfo(streamerName: string): Observable<LiveStreamDto | Error> {
    return this.httpClient
      .get<StreamInfoDto | Error>({
        controller: 'streams',
        action: 'live',
        routeParams: [streamerName],
      })
      .pipe(
        map(
          (res:any) =>
            ({
              startedAt: res.startedAt,
              options: res.streamOption,
              user: res.user,
            } as LiveStreamDto)
        ),
        catchError((err) => throwError(err.error as Error))
      );
  }
}

import { Injectable, inject } from '@angular/core';
import { StreamerProxyService } from '../proxy/streamer.proxy.service';
import { forkJoin, map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StreamerService {
  readonly #streamerProxyService = inject(StreamerProxyService);

  getRecommendedStreamers() {
    return forkJoin([
      this.#streamerProxyService.getFollowingStreamers(),
      this.#streamerProxyService.getLiveStreamers(),
    ]).pipe(
      map(([following, live]) => {
        console.log(following, live);
        // TODO: merge following and live, sort by following
        return [];
      })
    );
  }
}

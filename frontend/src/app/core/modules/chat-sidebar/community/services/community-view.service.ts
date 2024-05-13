import { inject, Injectable } from '@angular/core';
import { CommunityProxyService } from './community-proxy.service';
import { StreamFacade } from '../../../streams/services/stream.facade';
import { map, of } from 'rxjs';

@Injectable()
export class CommunityViewService {
  private readonly communityProxyService = inject(CommunityProxyService);

  private readonly streamFacade = inject(StreamFacade);

  getCurrentViewers() {
    const streamerName = this.streamFacade.streamerName() as string;

    return streamerName
      ? this.communityProxyService.getViewers(streamerName).pipe(
          map((viewers) => {
            const uniqueViewersMap = new Map();

            viewers.forEach((viewer) =>
              uniqueViewersMap.set(viewer.username, viewer)
            );

            return Array.from(uniqueViewersMap.values());
          })
        )
      : of([]);
  }

  getCurrentViewerCount() {
    const streamerName = this.streamFacade.streamerName() as string;

    return streamerName
      ? this.communityProxyService.getViewersCount(streamerName)
      : of(0);
  }
}

import { inject, Injectable } from '@angular/core';
import { CommunityProxyService } from './community-proxy.service';
import { map, of } from 'rxjs';

@Injectable()
export class CommunityViewService {
  private readonly communityProxyService = inject(CommunityProxyService);

  getCurrentViewers(streamerName?: string) {
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

  getCurrentViewerCount(streamerName?: string) {
    return streamerName
      ? this.communityProxyService.getViewersCount(streamerName)
      : of(undefined);
  }
}

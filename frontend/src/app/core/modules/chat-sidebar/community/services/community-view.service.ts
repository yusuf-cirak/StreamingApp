import { inject, Injectable } from '@angular/core';
import { CommunityProxyService } from './community-proxy.service';
import { map, of, tap } from 'rxjs';
import { AuthService } from '@streaming-app/core';

@Injectable({ providedIn: 'root' })
export class CommunityViewService {
  private readonly authService = inject(AuthService);
  private readonly communityProxyService = inject(CommunityProxyService);

  getIsBlockedFromStream(streamerId: string) {
    return this.communityProxyService.isUserBlocked(streamerId).pipe(
      tap((isBlocked) => {
        if (!isBlocked) {
          return;
        }

        this.authService.updateBlockedStreamers([
          ...this.authService.blockedStreamIds(),
          streamerId,
        ]);
      })
    );
  }

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

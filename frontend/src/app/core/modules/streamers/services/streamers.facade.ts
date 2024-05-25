import { computed, inject, Injectable } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { forkJoin, map, merge, of, switchMap } from 'rxjs';
import { AuthService } from '@streaming-app/core';
import { StreamerDto } from '../models/streamer-dto';
import { StreamHub } from '../../../hubs/stream-hub';
import { StreamProxyService } from '../../streams/services/stream-proxy.service';
import { StreamFollowerService } from '../../streams/services/stream-follower.service';

@Injectable({ providedIn: 'root' })
export class StreamersFacade {
  private readonly streamProxyService = inject(StreamProxyService);
  private readonly authService = inject(AuthService);
  private readonly streamHub = inject(StreamHub);

  readonly authenticated$ = toObservable(this.authService.isAuthenticated);
  readonly streamStarted$ = this.streamHub.streamStarted$;
  readonly streamEnded$ = this.streamHub.streamEnd$;
  readonly follow$ = inject(StreamFollowerService).follow$;

  readonly streamers = toSignal(
    merge(
      this.authenticated$,
      this.streamStarted$,
      this.streamEnded$,
      this.follow$ // todo: fix this
    ).pipe(switchMap(() => this.getStreamers()))
  );

  readonly followingStreamers = computed(
    () => this.streamers()?.followingStreamers || []
  );

  readonly recommendedStreamers = computed(
    () => this.streamers()?.recommendedStreamers || []
  );

  private getStreamers() {
    return forkJoin([
      this.streamProxyService.getLive(),
      this.getFollowingStreamers(),
      this.streamProxyService.getRecommended(),
      this.getBlockedStreamers(),
    ]).pipe(
      map(
        ([
          liveStreamersResult,
          followingStreamersResult,
          recommendedStreamersResult,
        ]) => {
          // prevent duplicates
          const liveStreamerIds = liveStreamersResult.map((ls) => ls.user.id);
          const allFollowingStreamerIds = Array.from(
            new Set([
              ...liveStreamerIds,
              ...followingStreamersResult.map((fs) => fs.user.id),
            ])
          );

          this.authService.updateFollowingStreamers(allFollowingStreamerIds);

          const allRecommendedStreamerIds = Array.from(
            new Set([
              ...liveStreamerIds,
              ...recommendedStreamersResult
                .filter((rs) => !allFollowingStreamerIds.includes(rs.user.id))
                .map((rs) => rs.user.id),
            ])
          );

          const followingStreamers = followingStreamersResult.filter((fs) =>
            allFollowingStreamerIds.includes(fs.user.id)
          );

          const recommendedStreamers = recommendedStreamersResult.filter((rs) =>
            allRecommendedStreamerIds.includes(rs.user.id)
          );
          return {
            recommendedStreamers,
            followingStreamers,
          };
        }
      )
    );
  }

  private getFollowingStreamers() {
    return this.authService.isAuthenticated()
      ? this.streamProxyService.getFollowing()
      : of([]);
  }

  private getBlockedStreamers() {
    return this.authService.isAuthenticated()
      ? this.streamProxyService.getBlocked()
      : of([]);
  }
}

export type AllStreamersDto = {
  recommendedStreamers: StreamerDto[];
  followingStreamers: StreamerDto[];
};

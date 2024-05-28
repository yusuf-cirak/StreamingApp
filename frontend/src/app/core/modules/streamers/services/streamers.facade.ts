import { computed, inject, Injectable } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { concatMap, forkJoin, map, merge, of, switchMap } from 'rxjs';
import { AuthService } from '@streaming-app/core';
import { StreamHub } from '../../../hubs/stream-hub';
import { StreamProxyService } from '../../streams/services/stream-proxy.service';
import { StreamFollowerService } from '../../streams/services/stream-follower.service';
import { StreamDto } from '../../streams/contracts/stream-dto';
import { StreamOptionService } from '../../streams/services/stream-option.service';

@Injectable({ providedIn: 'root' })
export class StreamersFacade {
  private readonly streamProxyService = inject(StreamProxyService);
  private readonly streamOptionService = inject(StreamOptionService);
  private readonly streamFollowerService = inject(StreamFollowerService);
  private readonly authService = inject(AuthService);
  private readonly streamHub = inject(StreamHub);

  readonly streamers = toSignal(
    merge(
      toObservable(this.authService.isAuthenticated),
      this.streamHub.streamStarted$,
      this.streamHub.streamEnd$,
      this.streamFollowerService.follow$, // todo: fix this
      this.streamOptionService.optionUpdate$
    ).pipe(concatMap(() => this.getStreamers()))
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
              ...followingStreamersResult
                .filter((fs) => !liveStreamerIds.includes(fs.user.id))
                .map((fs) => fs.user.id),
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
            allStreamers: [...followingStreamers, ...recommendedStreamers],
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
  recommendedStreamers: StreamDto[];
  followingStreamers: StreamDto[];
};
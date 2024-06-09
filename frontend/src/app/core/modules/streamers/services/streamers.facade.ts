import { computed, inject, Injectable } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { catchError, concatMap, forkJoin, map, merge, of } from 'rxjs';
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
      catchError(() => of([[], [], []])),
      map(
        ([
          liveStreamersResult,
          followingStreamersResult,
          recommendedStreamersResult,
        ]) => {
          // Get the list of live streamer IDs
          const liveStreamerIds = liveStreamersResult.map((ls) => ls.user.id);

          // Get the list of following streamer IDs
          const followingStreamerIds = followingStreamersResult.map(
            (fs) => fs.user.id
          );

          this.authService.updateFollowingStreamers(followingStreamerIds);

          // Determine following streamers that are live and not live
          const followingLiveStreamers = followingStreamersResult.filter((fs) =>
            liveStreamerIds.includes(fs.user.id)
          );
          const followingNotLiveStreamers = followingStreamersResult.filter(
            (fs) => !liveStreamerIds.includes(fs.user.id)
          );

          // Determine live streamers who are not being followed
          const notFollowingLiveStreamers = liveStreamersResult.filter(
            (ls) => !followingStreamerIds.includes(ls.user.id)
          );

          // Determine recommended streamers that are not being followed
          const recommendedNotFollowingStreamers =
            recommendedStreamersResult.filter(
              (rs) => !followingStreamerIds.includes(rs.user.id)
            );

          // Combine following streamers into one list
          const followingStreamers = [
            ...followingLiveStreamers,
            ...followingNotLiveStreamers,
          ];

          // Combine recommended streamers into one list and make them unique by user id
          const recommendedStreamersMap = new Map();

          // Add not following live streamers to the map
          notFollowingLiveStreamers.forEach((streamer) => {
            recommendedStreamersMap.set(streamer.user.id, streamer);
          });

          // Add recommended not following streamers to the map
          recommendedNotFollowingStreamers.forEach((streamer) => {
            recommendedStreamersMap.set(streamer.user.id, streamer);
          });

          // Convert the map back to an array
          const recommendedStreamers = Array.from(
            recommendedStreamersMap.values()
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

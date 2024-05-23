import { computed, inject, Injectable } from '@angular/core';
import { StreamersProxyService } from './streamers-proxy.service';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { forkJoin, map, merge, of, switchMap } from 'rxjs';
import { AuthService, User } from '@streaming-app/core';
import { StreamDto } from '../../streams/contracts/stream-dto';
import { StreamerDto } from '../models/streamer-dto';
import { StreamHub } from '../../../hubs/stream-hub';

@Injectable()
export class StreamersFacade {
  private readonly streamersProxyService = inject(StreamersProxyService);
  private readonly authService = inject(AuthService);
  private readonly streamHub = inject(StreamHub);

  readonly authenticated$ = toObservable(this.authService.isAuthenticated);
  readonly streamStarted$ = this.streamHub.streamStarted$;
  readonly streamEnded$ = this.streamHub.streamEnd$;

  readonly streamers = toSignal(
    // todo: onfollow, onunfollow
    merge(this.authenticated$, this.streamStarted$, this.streamEnded$).pipe(
      switchMap(() => this.getStreamers())
    )
  );

  readonly followingStreamers = computed(
    () => this.streamers()?.followingStreamers || []
  );

  readonly recommendedStreamers = computed(
    () => this.streamers()?.recommendedStreamers || []
  );

  private getStreamers() {
    return forkJoin([
      this.streamersProxyService.getLive(),
      this.getFollowingStreamers(),
    ]).pipe(
      map(([recommendedLiveStreamersResult, followingStreamersResult]) => {
        const allFollowingStreamers: User[] = followingStreamersResult.map(
          (fs) => fs.user
        );
        const followerStreamerIds = allFollowingStreamers.map((fs) => fs.id);

        this.authService.updateFollowingStreamers(followerStreamerIds);

        const recommendedStreamers = recommendedLiveStreamersResult.filter(
          (ls) => !followerStreamerIds.includes(ls.user.id)
        );

        // Filter following streamers into live and offline
        const liveFollowingStreamers = recommendedLiveStreamersResult.filter(
          (ls) => followerStreamerIds.includes(ls.user.id)
        );
        const offlineFollowingStreamers = allFollowingStreamers.filter(
          (fs) =>
            !liveFollowingStreamers.some(
              (lfs: StreamDto) => lfs.user.id === fs.id
            )
        );

        const followingStreamers: StreamerDto[] = [
          ...liveFollowingStreamers,
          ...offlineFollowingStreamers,
        ];

        return {
          recommendedStreamers,
          followingStreamers,
        };
      })
    );
  }

  private getFollowingStreamers() {
    return this.authService.isAuthenticated()
      ? this.streamersProxyService.getFollowing()
      : of([]);
  }
}

export type AllStreamersDto = {
  recommendedStreamers: StreamerDto[];
  followingStreamers: StreamerDto[];
};

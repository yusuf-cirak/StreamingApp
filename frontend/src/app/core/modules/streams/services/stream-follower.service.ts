import { inject, Injectable } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { Subject, tap } from 'rxjs';
import {
  StreamFollowDto,
  StreamFollowerProxyService,
} from './stream-follower-proxy.service';

@Injectable({ providedIn: 'root' })
export class StreamFollowerService {
  private readonly streamFollowerProxyService = inject(
    StreamFollowerProxyService
  );
  private readonly authService = inject(AuthService);

  readonly follow$ = new Subject<void>();

  follow(followDto: StreamFollowDto) {
    return this.streamFollowerProxyService.follow(followDto).pipe(
      tap(() => {
        this.follow$.next();
        const followingStreamers = this.authService.followingStreamIds() || [];
        this.authService.updateFollowingStreamers([
          ...followingStreamers,
          followDto.streamerId,
        ]);
      })
    );
  }

  unfollow(unfollowDto: StreamFollowDto) {
    return this.streamFollowerProxyService.unfollow(unfollowDto).pipe(
      tap(() => {
        this.follow$.next();
        this.authService.updateFollowingStreamers(
          this.authService
            .followingStreamIds()
            .filter((streamerId) => streamerId !== unfollowDto.streamerId)
        );
      })
    );
  }

  isFollowingStreamer(streamerId: string) {
    return this.streamFollowerProxyService.isFollowingStreamer(streamerId).pipe(
      tap((isFollowing) => {
        const followingStreamers = this.authService.followingStreamIds();
        if (isFollowing && !followingStreamers.includes(streamerId)) {
          this.authService.updateFollowingStreamers([
            ...followingStreamers,
            streamerId,
          ]);
        } else if (!isFollowing && followingStreamers.includes(streamerId)) {
          this.authService.updateFollowingStreamers(
            followingStreamers.filter((id) => id !== streamerId)
          );
        }
      })
    );
  }

  getFollowersCount(streamerId: string) {
    return this.streamFollowerProxyService.getFollowersCount(streamerId);
  }
}

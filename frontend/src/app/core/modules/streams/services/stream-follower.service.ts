import { inject, Injectable } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StreamFollowerService {
  private readonly httpClient = inject(HttpClientService);
  private readonly authService = inject(AuthService);

  follow(followDto: StreamFollowDto) {
    return this.httpClient
      .post(
        {
          controller: 'stream-follower-users',
        },
        followDto
      )
      .pipe(
        tap(() => {
          const followingStreamers =
            this.authService.followingStreamIds() || [];
          this.authService.updateFollowingStreamers([
            ...followingStreamers,
            followDto.streamerId,
          ]);
        })
      );
  }

  unfollow(unfollowDto: StreamFollowDto) {
    return this.httpClient
      .delete(
        {
          controller: 'stream-follower-users',
        },
        unfollowDto
      )
      .pipe(
        tap(() => {
          this.authService.updateFollowingStreamers(
            this.authService
              .followingStreamIds()
              .filter((streamerId) => streamerId !== unfollowDto.streamerId)
          );
        })
      );
  }

  isFollowingStreamer(streamerId: string) {
    return this.httpClient
      .get<boolean>({
        controller: 'stream-follower-users',
        routeParams: [streamerId],
      })
      .pipe(
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
}

export type StreamFollowDto = {
  streamerId: string;
  userId: string;
};

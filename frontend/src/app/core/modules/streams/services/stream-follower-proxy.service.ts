import { inject, Injectable } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StreamFollowerProxyService {
  private readonly httpClient = inject(HttpClientService);

  follow(followDto: StreamFollowDto) {
    return this.httpClient.post(
      {
        controller: 'stream-follower-users',
      },
      followDto
    );
  }

  unfollow(unfollowDto: StreamFollowDto) {
    return this.httpClient.delete(
      {
        controller: 'stream-follower-users',
      },
      unfollowDto
    );
  }

  isFollowingStreamer(streamerId: string) {
    return this.httpClient.get<boolean>({
      controller: 'stream-follower-users',
      routeParams: [streamerId],
    });
  }

  getFollowersCount(streamerId: string) {
    return this.httpClient.get<number>({
      controller: 'stream-follower-users',
      action: 'count',
      routeParams: [streamerId],
    });
  }
}

export type StreamFollowDto = {
  streamerId: string;
  userId: string;
};

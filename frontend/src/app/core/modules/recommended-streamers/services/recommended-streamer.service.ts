import { Injectable, inject } from '@angular/core';
import { StreamerProxyService } from './recommended-streamer.proxy.service';
import { forkJoin, map, of, tap } from 'rxjs';
import { StreamDto } from '../../streams/contracts/stream-dto';
import { FollowingStreamerDto } from '../models/following-stream-dto';
import { AuthService } from '@streaming-app/core';
import { UserProxyService } from '../../users/services/user-proxy.service';

@Injectable({
  providedIn: 'root',
})
export class RecommendedStreamersService {
  readonly streamerProxyService = inject(StreamerProxyService);
  readonly userProxyService = inject(UserProxyService);
  readonly authService = inject(AuthService);

  getFollowingStreamers() {
    return this.userProxyService.getFollowingStreamers();
  }

  getLiveStreamers() {
    return this.streamerProxyService.getLiveStreamers();
  }

  getRecommendedStreamers() {
    return forkJoin({
      liveStreamersResult: this.getLiveStreamers(),
      followingStreamersResult: this.authService.isAuthenticated()
        ? this.getFollowingStreamers()
        : of([]),
    }).pipe(
      map(({ liveStreamersResult, followingStreamersResult }) => {
        // Exclude live streamers from following streamers
        const liveStreamers = this.sortLiveStreamers(
          liveStreamersResult,
          followingStreamersResult
        );

        const followingStreamers = this.filterLiveStreamersFromFollowing(
          liveStreamers,
          followingStreamersResult
        );

        return { liveStreamers, followingStreamers };
      })
    );
  }

  private sortLiveStreamers(
    liveStreamers: StreamDto[],
    followingStreamers: FollowingStreamerDto[]
  ) {
    const followedStreamers = liveStreamers.filter((streamer) =>
      followingStreamers.some((fs) => fs.user.id === streamer.user.id)
    );
    const notFollowedStreamers = liveStreamers.filter(
      (streamer) =>
        !followingStreamers.some((fs) => fs.user.id === streamer.user.id)
    );

    return followedStreamers.concat(notFollowedStreamers);
  }

  private filterLiveStreamersFromFollowing(
    liveStreamers: StreamDto[],
    followingStreamers: FollowingStreamerDto[]
  ) {
    const filteredFollowingStreamers = followingStreamers.filter((streamer) =>
      liveStreamers.some(
        (liveStreamer) => liveStreamer.user.id !== streamer.user.id
      )
    );

    return filteredFollowingStreamers;
  }
}

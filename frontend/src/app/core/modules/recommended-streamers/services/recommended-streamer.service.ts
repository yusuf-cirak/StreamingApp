import { Injectable, inject } from '@angular/core';
import { StreamerProxyService } from './recommended-streamer.proxy.service';
import { forkJoin, map, of, tap } from 'rxjs';
import { LiveStreamDto } from '../models/live-stream-dto';
import { FollowingStreamerDto } from '../models/following-stream-dto';
import { AuthService } from '@streaming-app/core';

@Injectable({
  providedIn: 'root',
})
export class RecommendedStreamersService {
  readonly streamerProxyService = inject(StreamerProxyService);
  readonly authService = inject(AuthService);

  getFollowingStreamers() {
    return this.streamerProxyService.getFollowingStreamers();
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
    liveStreamers: LiveStreamDto[],
    followingStreamers: FollowingStreamerDto[]
  ) {
    const followedStreamers = liveStreamers.filter((streamer) =>
      followingStreamers.includes(streamer)
    );
    const notFollowedStreamers = liveStreamers.filter(
      (streamer) => !followingStreamers.includes(streamer)
    );

    return followedStreamers.concat(notFollowedStreamers);
  }

  private filterLiveStreamersFromFollowing(
    liveStreamers: LiveStreamDto[],
    followingStreamers: FollowingStreamerDto[]
  ) {
    const filteredFollowingStreamers = followingStreamers.filter((streamer) =>
      liveStreamers.some(
        (liveStreamer) => liveStreamer.user.id === streamer.user.id
      )
    );

    return filteredFollowingStreamers;
  }
}

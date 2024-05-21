import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { StreamService } from '../services/stream.service';
import { forkJoin, of, switchMap, tap } from 'rxjs';
import { StreamFacade } from '../services/stream.facade';
import { AuthService } from '@streaming-app/core';
import { StreamFollowerService } from '../services/stream-follower.service';
import { CommunityProxyService } from '../../chat-sidebar/community/services/community-proxy.service';
import { CommunityViewService } from '../../chat-sidebar/community/services/community-view.service';

export const streamStateResolver: ResolveFn<unknown> = (route) => {
  const authService = inject(AuthService);
  let streamerName = route.params['streamerName'];

  if (!streamerName) {
    streamerName =
      inject(AuthService).user()?.username ||
      inject(Router).getCurrentNavigation()?.extras.state!['streamerName']; // User state passed from the previous route
  }
  const streamFacade = inject(StreamFacade);

  streamFacade.setStreamerName(streamerName);

  const streamService = inject(StreamService);
  const streamFollowerService = inject(StreamFollowerService);
  const communityViewService = inject(CommunityViewService);

  return streamService.getStreamInfo(streamerName).pipe(
    tap((streamDto) => {
      streamFacade.setStream(streamDto);
    }),
    switchMap((streamDto) => {
      const streamer = streamDto.stream?.user;

      if (!streamer) {
        return of(null);
      }

      // if (streamFacade.canJoinToChatRoom()) {
      //   streamFacade.joinStreamRoom(streamer.username);
      // }

      const currentUser = authService.user();

      const streamBlockFollowInfo$ = currentUser
        ? forkJoin([
            communityViewService.getIsBlockedFromStream(streamer.id),
            streamFollowerService.isFollowingStreamer(streamer.id),
          ])
        : of(null);

      return streamBlockFollowInfo$;
    })
  );
};

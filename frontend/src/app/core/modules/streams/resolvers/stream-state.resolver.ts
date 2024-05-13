import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { StreamService } from '../services/stream.service';
import { EMPTY, forkJoin, of, switchMap, tap } from 'rxjs';
import { StreamFacade } from '../services/stream.facade';
import { AuthService } from '@streaming-app/core';
import { StreamFollowerService } from '../services/stream-follower.service';

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

  return streamService.getStreamInfo(streamerName).pipe(
    tap((streamDto) => {
      streamFacade.setStream(streamDto);
    }),
    switchMap((streamDto) => {
      const streamer = streamDto.stream?.user;
      const currentUser = authService.user();
      if (!streamer || !currentUser) {
        return of(null);
      }

      if (streamFacade.canJoinToChatRoom()) {
        streamFacade.joinStreamRoom(streamer.username);
      }

      return streamFollowerService.isFollowingStreamer(streamer.id);
    })
  );
};

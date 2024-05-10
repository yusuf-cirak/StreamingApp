import { inject } from '@angular/core';
import { ResolveFn, Router } from '@angular/router';
import { StreamService } from '../services/stream.service';
import { StreamDto } from '../contracts/stream-dto';
import { catchError, of, tap } from 'rxjs';
import { Error } from '../../../../shared/api/error';
import { StreamFacade } from '../services/stream.facade';
import { AuthService } from '@streaming-app/core';
import { StreamState } from '../models/stream-state';

export const streamStateResolver: ResolveFn<StreamState> = (route) => {
  let streamerName = route.params['streamerName'];

  if (!streamerName) {
    streamerName =
      inject(AuthService).user()?.username ||
      inject(Router).getCurrentNavigation()?.extras.state!['streamerName']; // User state passed from the previous route
  }
  const streamFacade = inject(StreamFacade);

  streamFacade.setStreamerName(streamerName);

  const streamService = inject(StreamService);

  return streamService.getStreamInfo(streamerName).pipe(
    tap((streamDto) => {
      streamFacade.setStream(streamDto);
    })
    // catchError((err) => {
    //   streamFacade.setLiveStreamErrorState(err as Error);
    //   return of(err as Error);
    // })
  );
};

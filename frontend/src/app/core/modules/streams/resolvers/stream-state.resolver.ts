import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { StreamService } from '../services/stream.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { catchError, of, tap } from 'rxjs';
import { Error } from '../../../../shared/api/error';
import { StreamFacade } from '../services/stream.facade';

export const streamStateResolver: ResolveFn<LiveStreamDto | Error> = (route, state) => {
  const { streamerName } = route.params;
  const streamService = inject(StreamService);
  const streamFacade = inject(StreamFacade)

  return streamService
    .getStreamInfo(streamerName).pipe(tap((liveStream)=>{
      streamFacade.setLiveStream(liveStream as LiveStreamDto);
    }),catchError((err) => {
      streamFacade.setLiveStreamErrorState(err as Error);
     return of(err as Error)
    }));
};

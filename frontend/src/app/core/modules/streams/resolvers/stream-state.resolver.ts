import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { StreamService } from '../services/stream.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';

export const streamStateResolver: ResolveFn<LiveStreamDto> = (route, state) => {
  const { streamerName } = route.params;
  const streamService = inject(StreamService);

  return streamService.getStreamInfo(streamerName);
};

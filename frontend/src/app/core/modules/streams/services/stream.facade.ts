import { Injectable, inject } from '@angular/core';
import { StreamService } from './stream.service';

@Injectable()
export class StreamFacade {
  readonly streamService = inject(StreamService);

  getStreamInfo(streamerName: string) {
    return this.streamService.getStreamInfo(streamerName);
  }

  getHlsUrl(streamKey: string) {
    return this.streamService.getHlsUrl(streamKey);
  }
}

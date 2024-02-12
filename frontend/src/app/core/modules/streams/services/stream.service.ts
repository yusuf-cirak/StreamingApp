import { Injectable, inject } from '@angular/core';
import { StreamProxyService } from './stream-proxy.service';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class StreamService {
  readonly streamProxyService = inject(StreamProxyService);

  getStreamInfo(streamerName: string) {
    return this.streamProxyService.getStreamInfo(streamerName);
  }

  getHlsUrl(streamKey: string) {
    return `${environment.hlsUrl}/${streamKey}.m3u8`;
  }
}

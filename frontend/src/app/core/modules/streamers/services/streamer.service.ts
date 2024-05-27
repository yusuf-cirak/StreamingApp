import { Injectable } from '@angular/core';
import { StreamDto } from '../../streams/contracts/stream-dto';

@Injectable({
  providedIn: 'root',
})
export class StreamerService {
  isLive(streamer: StreamDto) {
    return !!streamer.streamOption.streamKey?.length;
  }
}

import { Injectable, Signal, inject, signal } from '@angular/core';
import { StreamerService } from './streamer.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class StreamerFacade {
  readonly #streamerService = inject(StreamerService);

  streamers = toSignal(this.#streamerService.getRecommendedStreamers());
}

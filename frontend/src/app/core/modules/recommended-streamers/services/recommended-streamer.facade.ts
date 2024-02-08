import { Injectable, Signal, inject, signal } from '@angular/core';
import { RecommendedStreamersService } from './recommended-streamer.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RecommendedStreamersFacade {
  readonly #streamerService = inject(RecommendedStreamersService);

  recommendedStreamers = toSignal(
    this.#streamerService.getRecommendedStreamers()
  );
}

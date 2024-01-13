import { Injectable, signal } from '@angular/core';
import { Streamer } from '../models/streamer';

@Injectable({
  providedIn: 'root',
})
export class StreamerFacade {
  #streamers = signal<Streamer[] | undefined>(undefined);

  readonly streamers = this.#streamers.asReadonly();
}

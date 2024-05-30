import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../../../models';

@Injectable({ providedIn: 'root' })
export class CreatorService {
  // readonly moderatingStreamers = signal<User[]>([]);

  #streamer = signal<User | undefined>(undefined);
  readonly streamer = this.#streamer.asReadonly();

  readonly streamerId = computed(() => this.#streamer()?.id);
  readonly streamerName = computed(() => this.#streamer()?.username);

  setStreamer(streamer: User) {
    this.#streamer.set(streamer);
  }
}

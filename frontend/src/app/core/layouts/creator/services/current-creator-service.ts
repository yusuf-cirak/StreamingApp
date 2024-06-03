import { computed, Injectable, signal } from '@angular/core';
import { User } from '../../../models';

@Injectable({ providedIn: 'root' })
export class CurrentCreatorService {
  #streamer = signal<User | undefined>(undefined);
  readonly streamer = this.#streamer.asReadonly();

  readonly streamerId = computed(() => this.#streamer()?.id);
  readonly streamerName = computed(() => this.#streamer()?.username);

  update(streamer: User) {
    this.#streamer.set(streamer);
  }
}

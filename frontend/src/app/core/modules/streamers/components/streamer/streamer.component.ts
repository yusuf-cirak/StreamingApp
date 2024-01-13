import { Component, Input, signal } from '@angular/core';
import { Streamer } from '../../models/streamer';
import { NgClass } from '@angular/common';

@Component({
  standalone: true,
  imports: [NgClass],
  templateUrl: './streamer.component.html',
  selector: 'app-streamer',
})
export class StreamerComponent {
  readonly #streamer = signal<Streamer | undefined>(undefined);
  readonly streamer = this.#streamer.asReadonly();

  @Input('streamer') set streamerSetter(streamer: Streamer) {
    this.#streamer.set(streamer);
  }
}

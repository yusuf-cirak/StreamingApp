import { Component, Input, signal } from '@angular/core';
import { NgClass, NgIf } from '@angular/common';
import { LiveStream } from '../../models/live-stream';

@Component({
  standalone: true,
  imports: [NgClass, NgIf],
  templateUrl: './streamer.component.html',
  selector: 'app-streamer',
})
export class StreamerComponent {
  readonly #liveStream = signal<LiveStream | undefined>(undefined);
  readonly liveStream = this.#liveStream.asReadonly();

  @Input('liveStream') set streamerSetter(streamer: LiveStream) {
    this.#liveStream.set(streamer);
  }
}

import { Component, Input, signal } from '@angular/core';
import { StreamerComponent } from '../streamer/streamer.component';
import { Streamer } from '../../models/streamer';
import { LiveStream } from '../../models/live-stream';

@Component({
  templateUrl: './streamer-list.component.html',
  standalone: true,
  selector: 'app-streamer-list',
  imports: [StreamerComponent],
})
export class StreamerListComponent {
  #streamers = signal<LiveStream[]>([]);

  readonly streamers = this.#streamers.asReadonly();

  @Input('streamers') set streamersSetter(streamers: LiveStream[]) {
    this.#streamers.set(streamers);
  }
}

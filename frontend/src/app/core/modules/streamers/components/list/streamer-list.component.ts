import { Component, Input, signal } from '@angular/core';
import { StreamerComponent } from '../streamer/streamer.component';
import { Streamer } from '../../models/streamer';

@Component({
  templateUrl: './streamer-list.component.html',
  standalone: true,
  selector: 'app-streamer-list',
  imports: [StreamerComponent],
})
export class StreamerListComponent {
  #streamers = signal<Streamer[]>([]);

  readonly streamers = this.#streamers.asReadonly();

  @Input('streamers') set streamersSetter(streamers: Streamer[]) {
    this.#streamers.set(streamers);
  }
}

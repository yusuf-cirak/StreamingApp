import { Component, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { LiveStreamDto } from '../../models/live-stream-dto';

@Component({
  standalone: true,
  imports: [NgClass],
  templateUrl: './live-streamer.component.html',
  selector: 'app-live-streamer',
})
export class LiveStreamerComponent {
  readonly liveStreamer = input.required<LiveStreamDto>();
}

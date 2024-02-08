import { Component, input } from '@angular/core';
import { FollowingStreamerDto } from '../../models/following-stream-dto';
import { LiveStreamDto } from '../../models/live-stream-dto';
import { LiveStreamerComponent } from '../live-stream/live-streamer.component';
import { FollowingStreamerComponent } from '../following-streamer/following-streamer.component';

@Component({
  templateUrl: './streamer-list.component.html',
  standalone: true,
  selector: 'app-streamer-list',
  imports: [LiveStreamerComponent, FollowingStreamerComponent],
})
export class StreamerListComponent {
  readonly liveStreamers = input.required<LiveStreamDto[]>();
  readonly followingStreamers = input.required<FollowingStreamerDto[]>();
}

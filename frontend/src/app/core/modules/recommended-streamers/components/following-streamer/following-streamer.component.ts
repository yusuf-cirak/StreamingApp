import { Component, input } from '@angular/core';
import { NgClass, NgIf } from '@angular/common';
import { FollowingStreamerDto } from '../../models/following-stream-dto';

@Component({
  standalone: true,
  imports: [NgClass],
  templateUrl: './following-streamer.component.html',
  selector: 'app-following-streamer',
})
export class FollowingStreamerComponent {
  readonly followingStreamer = input.required<FollowingStreamerDto>();
}

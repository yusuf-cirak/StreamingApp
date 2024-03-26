import { Component, inject, input } from '@angular/core';
import { NgClass, NgIf } from '@angular/common';
import { FollowingStreamerDto } from '../../models/following-stream-dto';
import { RouterLink } from '@angular/router';
import { UserImageService } from '../../../../services/user-image.service';

@Component({
  standalone: true,
  imports: [NgClass, RouterLink],
  templateUrl: './following-streamer.component.html',
  selector: 'app-following-streamer',
})
export class FollowingStreamerComponent {
  readonly userImageService = inject(UserImageService);
  readonly followingStreamer = input.required<FollowingStreamerDto>();
}

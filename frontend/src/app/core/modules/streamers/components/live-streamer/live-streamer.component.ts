import { Component, inject, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { RouterLink } from '@angular/router';
import { StreamerAvatarComponent } from '../streamer-avatar/streamer-avatar.component';
import { ImageService } from '../../../../services/image.service';

@Component({
  standalone: true,
  imports: [NgClass, RouterLink, StreamerAvatarComponent],
  templateUrl: './live-streamer.component.html',
  selector: 'app-live-streamer',
})
export class LiveStreamerComponent {
  readonly userImageService = inject(ImageService);
  readonly liveStreamer = input.required<StreamDto>();
}

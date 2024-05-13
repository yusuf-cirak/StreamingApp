import { Component, inject, input } from '@angular/core';
import { NgClass } from '@angular/common';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { RouterLink } from '@angular/router';
import { UserImageService } from '../../../../services/user-image.service';
import { StreamerComponent } from '../streamer/streamer.component';

@Component({
  standalone: true,
  imports: [NgClass, RouterLink, StreamerComponent],
  templateUrl: './live-streamer.component.html',
  selector: 'app-live-streamer',
})
export class LiveStreamerComponent {
  readonly userImageService = inject(UserImageService);
  readonly liveStreamer = input.required<StreamDto>();
}

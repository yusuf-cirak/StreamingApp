import { Component, inject, input } from '@angular/core';
import { User } from '../../../../models';
import { NgClass } from '@angular/common';
import { ImageService } from '../../../../services/image.service';

@Component({
  selector: 'app-streamer',
  standalone: true,
  imports: [NgClass],
  templateUrl: './streamer-image.component.html',
})
export class StreamerImageComponent {
  readonly imageService = inject(ImageService);

  readonly isLive = input(false);

  readonly user = input.required<User>();
}

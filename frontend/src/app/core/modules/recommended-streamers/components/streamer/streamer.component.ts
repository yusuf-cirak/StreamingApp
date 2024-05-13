import { Component, inject, input } from '@angular/core';
import { UserImageService } from '../../../../services/user-image.service';
import { User } from '../../../../models';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-streamer',
  standalone: true,
  imports: [NgClass],
  templateUrl: './streamer.component.html',
})
export class StreamerComponent {
  readonly userImageService = inject(UserImageService);

  readonly isLive = input(false);

  readonly user = input.required<User>();
}

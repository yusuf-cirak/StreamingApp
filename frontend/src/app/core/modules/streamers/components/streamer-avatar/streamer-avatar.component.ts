import { Component, inject, input, signal } from '@angular/core';
import { User } from '../../../../models';
import { NgClass } from '@angular/common';
import { ImageService } from '../../../../services/image.service';
import { StreamerAvatarSkeletonComponent } from './streamer-avatar-skeleton/streamer-avatar-skeleton.component';

@Component({
  selector: 'app-streamer-avatar',
  standalone: true,
  imports: [NgClass, StreamerAvatarSkeletonComponent],
  templateUrl: './streamer-avatar.component.html',
})
export class StreamerAvatarComponent {
  readonly imageService = inject(ImageService);

  readonly user = input.required<User>();

  readonly isLive = input(false);

  readonly #isLoaded = signal(false);
  readonly isLoaded = this.#isLoaded.asReadonly();

  setIsLoaded(value: boolean) {
    this.#isLoaded.set(value);
  }
}

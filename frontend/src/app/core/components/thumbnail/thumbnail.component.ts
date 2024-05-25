import { Component, computed, inject, input } from '@angular/core';
import { StreamDto } from '../../modules/streams/contracts/stream-dto';
import { StreamerAvatarComponent } from '../../modules/streamers/components/streamer-avatar/streamer-avatar.component';
import { NgOptimizedImage, NgTemplateOutlet } from '@angular/common';
import { ImageService } from '../../services/image.service';
import { ThumbnailSkeletonComponent } from './thumbnail-skeleton/thumbnail-skeleton.component';

@Component({
  selector: 'app-thumbnail',
  standalone: true,
  imports: [
    StreamerAvatarComponent,
    NgTemplateOutlet,
    NgOptimizedImage,
    ThumbnailSkeletonComponent,
  ],
  templateUrl: './thumbnail.component.html',
})
export class ThumbnailComponent {
  streamer = input.required<StreamDto>();

  isLive = computed(() =>
    !!this.streamer()?.user
      ? !!(this.streamer() as StreamDto).streamOption?.streamKey
      : false
  );

  readonly imageService = inject(ImageService);
}

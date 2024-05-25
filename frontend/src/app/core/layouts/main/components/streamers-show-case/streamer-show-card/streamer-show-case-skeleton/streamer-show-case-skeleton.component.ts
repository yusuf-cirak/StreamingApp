import { Component } from '@angular/core';
import { ThumbnailSkeletonComponent } from '../../../../../../components/thumbnail/thumbnail-skeleton/thumbnail-skeleton.component';
import { StreamerAvatarSkeletonComponent } from '../../../../../../modules/streamers/components/streamer-avatar/streamer-avatar-skeleton/streamer-avatar-skeleton.component';

@Component({
  selector: 'app-streamer-show-case-skeleton',
  standalone: true,
  imports: [ThumbnailSkeletonComponent, StreamerAvatarSkeletonComponent],
  templateUrl: './streamer-show-case-skeleton.component.html',
})
export class StreamerShowCaseSkeletonComponent {}

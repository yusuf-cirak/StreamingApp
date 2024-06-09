import { Component, input } from '@angular/core';

@Component({
  standalone: true,
  templateUrl: './streamer-skeleton.component.html',
  selector: 'app-streamer-skeleton',
})
export class StreamerSkeletonComponent {
  readonly skeletonCount = input(Array.from({ length: 3 }).fill(0));
}

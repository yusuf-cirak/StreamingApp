import { Component, Input, signal } from '@angular/core';

@Component({
  standalone: true,
  templateUrl: './streamer-skeleton.component.html',
  selector: 'app-streamer-skeleton',
})
export class StreamerSkeletonComponent {
  readonly #skeletonCount = signal(Array.from({ length: 5 }).fill(0));

  readonly skeletonCount = this.#skeletonCount.asReadonly();

  @Input('skeletonCount') set skeletonCountSetter(count: number) {
    this.#skeletonCount.set(Array.from({ length: count }).fill(0));
  }
}

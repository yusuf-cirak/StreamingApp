import { Component } from '@angular/core';
import { SkeletonModule } from 'primeng/skeleton';

@Component({
  selector: 'app-stream-skeleton',
  standalone: true,
  imports: [SkeletonModule],
  templateUrl: './stream-skeleton.component.html',
})
export class StreamSkeletonComponent {}

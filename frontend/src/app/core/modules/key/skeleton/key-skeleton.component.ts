import { Component } from '@angular/core';
import { SkeletonModule } from 'primeng/skeleton';
@Component({
  standalone: true,
  selector: 'app-key-skeleton',
  templateUrl: './key-skeleton.component.html',
  imports: [SkeletonModule],
})
export class KeySkeletonComponent {}

import { Component } from '@angular/core';
import { SkeletonModule } from 'primeng/skeleton';
@Component({
  standalone: true,
  selector: 'app-chat-settings-skeleton',
  templateUrl: './chat-settings-skeleton.component.html',
  imports: [SkeletonModule],
})
export class ChatSettingsSkeletonComponent {}

import { Component, input, signal } from '@angular/core';
import { VgApiService, VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { VgStreamingModule } from '@videogular/ngx-videogular/streaming';
import { ChatSidebarComponent } from '../chat-sidebar/chat-sidebar.component';
import streamInfo from '../../../../assets/stream.json';
import { SkeletonModule } from 'primeng/skeleton';
import { StreamSkeletonComponent } from './components/stream-skeleton/stream-skeleton.component';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-stream',
  standalone: true,
  templateUrl: './stream.component.html',
  imports: [
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    VgStreamingModule,
    ChatSidebarComponent,
    SkeletonModule,
    StreamSkeletonComponent,
    NgTemplateOutlet,
  ],
})
export class StreamComponent {
  streamerName = input.required<string>();
  hlsUrl = signal<string | undefined>(undefined);

  onPlayerReady(service: VgApiService) {
    service.getDefaultMedia().subscriptions.loadedData.subscribe({
      next: () => {
        service.play();
      },
    });
  }
}

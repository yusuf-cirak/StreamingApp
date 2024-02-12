import { ActivatedRoute } from '@angular/router';
import { Component, inject } from '@angular/core';
import { VgApiService, VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { VgStreamingModule } from '@videogular/ngx-videogular/streaming';
import { ChatSidebarComponent } from '../chat-sidebar/chat-sidebar.component';
import { SkeletonModule } from 'primeng/skeleton';
import { StreamSkeletonComponent } from './components/stream-skeleton/stream-skeleton.component';
import { NgTemplateOutlet } from '@angular/common';
import { LiveStreamDto } from '../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from './services/stream.facade';
import { map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

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
  providers: [StreamFacade],
})
export class StreamComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly route = inject(ActivatedRoute);

  streamState = toSignal<LiveStreamDto>(
    this.route.data.pipe(
      map(({ streamState }) => {
        return streamState;
      })
    )
  );

  onPlayerReady(service: VgApiService) {
    service.getDefaultMedia().subscriptions.loadedData.subscribe({
      next: () => {
        service.play();
      },
    });
  }
}

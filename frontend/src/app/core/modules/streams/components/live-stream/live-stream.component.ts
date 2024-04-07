import { Component, inject, input } from '@angular/core';
import { VgApiService, VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { VgStreamingModule } from '@videogular/ngx-videogular/streaming';
import { LiveStreamDto } from '../../../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from '../../services/stream.facade';
import { ChatSidebarComponent } from '../../../chat-sidebar/chat-sidebar.component';

@Component({
  selector: 'app-live-stream',
  standalone: true,
  templateUrl: './live-stream.component.html',
  imports: [
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    VgStreamingModule,
    ChatSidebarComponent,
  ],
})
export class LiveStreamComponent {
  liveStream = input.required<LiveStreamDto>();

  readonly streamFacade = inject(StreamFacade);

  onPlayerReady(service: VgApiService) {
    service.getDefaultMedia().subscriptions.loadedData.subscribe({
      next: () => {
        service.play();
      },
    });
  }

  ngOnDestroy() {
    this.streamFacade.leaveStream();
  }
}

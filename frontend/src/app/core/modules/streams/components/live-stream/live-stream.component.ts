import { ChatSidebar } from '../../../chat-sidebar/models/chat-sidebar';
import { Component } from '@angular/core';
import { VgApiService, VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { VgStreamingModule } from '@videogular/ngx-videogular/streaming';
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
  onPlayerReady(service: VgApiService) {
    setTimeout(() => service.play(), 1500);
  }
}

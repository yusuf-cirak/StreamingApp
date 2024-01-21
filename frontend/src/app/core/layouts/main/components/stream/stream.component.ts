import { Component, ViewChild, signal } from '@angular/core';
import {
  BitrateOptions,
  VgApiService,
  VgCoreModule,
  VgPlayerComponent,
} from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { VgStreamingModule } from '@videogular/ngx-videogular/streaming';

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
  ],
})
export class StreamComponent {
  onPlayerReady(service: VgApiService) {
    setTimeout(() => service.play(), 1500);
  }
}

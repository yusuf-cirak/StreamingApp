import { OfflineStreamComponent } from './components/offline-stream/offline-stream.component';
import { Component, computed, inject } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { LiveStreamComponent } from './components/live-stream/live-stream.component';
import { StreamFacade } from './services/stream.facade';
import { NotFoundStreamComponent } from './components/not-found-stream/not-found-stream.component';

@Component({
  selector: 'app-stream',
  standalone: true,
  templateUrl: './stream.component.html',
  imports: [
    NgTemplateOutlet,
    LiveStreamComponent,
    OfflineStreamComponent,
    NotFoundStreamComponent,
  ],
})
export class StreamComponent {
  readonly streamFacade = inject(StreamFacade);

  readonly canJoinToChatRoom = computed(
    () =>
      this.streamFacade.streamState()?.error?.statusCode !== 404 ||
      this.streamFacade.liveStream()
  );

  ngOnInit() {
    if (this.canJoinToChatRoom()) {
      this.streamFacade.joinStreamRoom(this.streamFacade.streamerName());
    }
  }

  ngOnDestroy() {
    if (this.canJoinToChatRoom()) {
      this.streamFacade.leaveStreamRoom(this.streamFacade.streamerName());
    }
  }
}

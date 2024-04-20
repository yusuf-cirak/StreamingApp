import { OfflineStreamComponent } from './components/offline-stream/offline-stream.component';
import { Component, computed, inject } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { LiveStreamComponent } from './components/live-stream/live-stream.component';
import { StreamFacade } from './services/stream.facade';
import { NotFoundStreamComponent } from './components/not-found-stream/not-found-stream.component';
import { StreamHub } from '../../hubs/stream-hub';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';

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
  readonly streamHub = inject(StreamHub);
  readonly streamFacade = inject(StreamFacade);

  readonly streamError = computed(() => this.streamFacade.streamState()?.error);
  readonly router = inject(Router);

  readonly canJoinToChatRoom = computed(
    () =>
      this.streamFacade.streamState()?.error?.statusCode !== 404 ||
      this.streamFacade.liveStream()
  );

  constructor() {
    this.streamHub.streamStarted$.subscribe({
      next: (value) => {
        this.streamFacade.setLiveStream(value);

        //TODO: Add it from current live streamers
      },
    });

    this.streamHub.streamEnd$.pipe(takeUntilDestroyed()).subscribe({
      next: () => {
        this.streamFacade.endStream();
        //TODO: Delete it from current live streamers
      },
    });

    this.streamHub.streamChatOptionsChanged$
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: (value) => {
          console.log('Stream chat options changed');
          this.streamFacade.setStreamChatOptions(value);
        },
      });
  }

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

import { OfflineStreamComponent } from './components/offline-stream/offline-stream.component';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { LiveStreamComponent } from './components/live-stream/live-stream.component';
import { StreamFacade } from './services/stream.facade';
import { NotFoundStreamComponent } from './components/not-found-stream/not-found-stream.component';
import { StreamHub } from '../../hubs/stream-hub';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { AuthService } from '../../services';
import { StreamFollowerService } from './services/stream-follower.service';
import { CurrentCreatorService } from '../../layouts/creator/services/current-creator-service';
import { skip, tap } from 'rxjs';
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
  readonly authService = inject(AuthService);
  readonly streamFollowerService = inject(StreamFollowerService);

  readonly currentCreatorService = inject(CurrentCreatorService);

  readonly streamError = computed(() => this.streamFacade.streamState()?.error);

  readonly canJoinToChatRoom = this.streamFacade.canJoinToChatRoom;

  readonly router = inject(Router);

  constructor() {
    this.listenToHubEvents();

    toObservable(this.streamFacade.streamerName)
      .pipe(
        takeUntilDestroyed(),
        skip(1),
        tap((streamerName) => {
          this.streamFacade.leaveStreamRoom(streamerName);
          this.streamFacade.clearChatMessages();
          this.streamFacade.joinStreamRoom(streamerName);
        })
      )
      .subscribe();

    effect(() => {
      const isAuthenticated = this.authService.isAuthenticated();
      if (this.canJoinToChatRoom()) {
        this.streamFacade.joinStreamRoom(this.streamFacade.streamerName());
      }
    });
  }

  private listenToHubEvents() {
    this.streamHub.streamStarted$.subscribe({
      next: (value) => {
        this.streamFacade.updateStream(value);

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

  ngOnDestroy() {
    if (this.canJoinToChatRoom()) {
      this.streamFacade.leaveStreamRoom(this.streamFacade.streamerName());
    }
    this.streamFacade.clearChatMessages();
  }
}

import { Component, computed, inject, signal } from '@angular/core';
import { StreamFacade } from '../../services/stream.facade';
import { AuthService } from '../../../../services';
import { HeartIcon } from '../../../../../shared/icons/heart-icon';
import { NgClass } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import {
  StreamFollowDto,
  StreamFollowerService,
} from '../../services/stream-follower.service';
import { filter, forkJoin, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-stream-actions',
  standalone: true,
  imports: [HeartIcon, NgClass, RippleModule],
  templateUrl: './stream-actions.component.html',
})
export class StreamActionsComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly authService = inject(AuthService);

  readonly streamFollowerService = inject(StreamFollowerService);

  isFollowingStreamer = computed(() =>
    this.streamFacade.isFollowingLiveStreamer()
  );

  isSelf = computed(
    () => this.authService.user()?.username === this.streamFacade.streamerName()
  );

  isPendingFollow = signal(false);

  constructor() {
    this.authService.login$
      .pipe(
        takeUntilDestroyed(),
        filter(() => this.streamFacade.isStreamerExists()),
        switchMap(() => {
          this.isPendingFollow.set(true);
          return forkJoin([
            this.streamFollowerService.isFollowingStreamer(
              this.streamFacade.liveStream()?.user.id
            ),
            // this.streamFacade.getStreamChatOptions(),
            //todo: check if user is blocked here?
          ]);
        }),
        tap(([isFollowing]) => {
          if (isFollowing) {
            this.authService.updateFollowingStreamers([
              ...this.authService.followingStreamIds(),
              this.streamFacade.liveStream()?.user.id,
            ]);
          }
          this.isPendingFollow.set(false);
        })
      )
      .subscribe();
  }

  toggleFollow() {
    this.isPendingFollow.set(true);
    const isFollowing = this.isFollowingStreamer();

    const userId = this.authService.user()?.id as string;
    const streamerId = this.streamFacade.liveStream().user.id;

    const followDto: StreamFollowDto = { streamerId, userId };
    if (!isFollowing) {
      this.streamFollowerService
        .follow(followDto)
        .pipe(tap(() => this.isPendingFollow.set(false)))
        .subscribe();
    } else {
      this.streamFollowerService
        .unfollow(followDto)
        .pipe(tap(() => this.isPendingFollow.set(false)))
        .subscribe();
    }
  }
}

import { Component, computed, inject, signal } from '@angular/core';
import { StreamFacade } from '../../services/stream.facade';
import { AuthService } from '../../../../services';
import { HeartIcon } from '../../../../../shared/icons/heart-icon';
import { NgClass } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import { StreamFollowerService } from '../../services/stream-follower.service';
import { StreamFollowDto } from '../../services/stream-follower-proxy.service';
import { tap } from 'rxjs';

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

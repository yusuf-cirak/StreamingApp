import { Component, computed, effect, inject, signal } from '@angular/core';
import { StreamFacade } from '../../services/stream.facade';
import { UserIcon, VerifiedIcon } from '@streaming-app/shared/icons';
import { StreamActionsComponent } from '../stream-actions/stream-actions.component';
import { StreamFollowerService } from '../../services/stream-follower.service';
import { CommunityViewService } from '../../../chat-sidebar/community/services/community-view.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommunityProxyService } from '../../../chat-sidebar/community/services/community-proxy.service';
import { interval, Observable, of, startWith, Subject, switchMap } from 'rxjs';
import { StreamerAvatarComponent } from '../../../streamers/components/streamer-avatar/streamer-avatar.component';

@Component({
  selector: 'app-stream-header',
  standalone: true,
  imports: [
    StreamerAvatarComponent,
    StreamActionsComponent,
    VerifiedIcon,
    UserIcon,
  ],
  providers: [
    StreamFollowerService,
    CommunityViewService,
    CommunityProxyService,
  ],
  templateUrl: './stream-header.component.html',
})
export class StreamHeaderComponent {
  readonly streamFacade = inject(StreamFacade);

  readonly communityViewerService = inject(CommunityViewService);

  readonly streamOptions = this.streamFacade.streamOptions;

  readonly viewerCount$ = new Subject<Observable<number | undefined>>();

  readonly viewerCount = toSignal(
    this.viewerCount$.pipe(switchMap((source) => source))
  );

  constructor() {
    effect(
      () => {
        if (this.streamFacade.isStreamLive()) {
          this.viewerCount$.next(this.getCurrentViewerCount());
        } else {
          this.viewerCount$.next(of(undefined));
        }
      },
      { allowSignalWrites: true }
    );
  }

  getCurrentViewerCount() {
    return interval(10000).pipe(
      startWith(0),
      switchMap(() =>
        this.communityViewerService.getCurrentViewerCount(
          this.streamFacade.liveStream().user.username
        )
      )
    );
  }
}

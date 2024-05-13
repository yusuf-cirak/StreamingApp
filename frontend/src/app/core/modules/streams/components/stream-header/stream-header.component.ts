import { Component, computed, inject, signal } from '@angular/core';
import { StreamFacade } from '../../services/stream.facade';
import { UserIcon, VerifiedIcon } from '@streaming-app/shared/icons';
import { StreamerComponent } from '../../../recommended-streamers/components/streamer/streamer.component';
import { StreamActionsComponent } from '../stream-actions/stream-actions.component';
import { StreamFollowerService } from '../../services/stream-follower.service';
import { CommunityViewService } from '../../../chat-sidebar/community/services/community-view.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommunityProxyService } from '../../../chat-sidebar/community/services/community-proxy.service';
import { interval, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-stream-header',
  standalone: true,
  imports: [StreamerComponent, StreamActionsComponent, VerifiedIcon, UserIcon],
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

  readonly streamDescription = computed(
    () => this.streamFacade.liveStream().streamOption.description
  );

  readonly viewerCount = toSignal(this.getCurrentViewerCount());

  getCurrentViewerCount() {
    return interval(10000).pipe(
      startWith(0),
      switchMap(() => this.communityViewerService.getCurrentViewerCount())
    );
  }
}

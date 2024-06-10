import { Component, computed, inject } from '@angular/core';
import { VerifiedIcon } from '../../../../../shared/icons/verified-icon';
import { StreamFacade } from '../../services/stream.facade';
import { StreamFollowerService } from '../../services/stream-follower.service';
import { interval, startWith, switchMap } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-stream-about',
  standalone: true,
  imports: [VerifiedIcon],
  templateUrl: './stream-about.component.html',
})
export class StreamAboutComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly streamFollowerService = inject(StreamFollowerService);

  readonly streamer = computed(() => this.streamFacade.streamState().stream);
  readonly streamOptions = this.streamFacade.streamOptions;

  readonly streamFollowerCount = toSignal(this.getFollowersCount());

  private getFollowersCount() {
    return interval(30000).pipe(
      startWith(0),
      switchMap(() =>
        this.streamFollowerService.getFollowersCount(this.streamer()?.user.id!)
      )
    );
  }
}

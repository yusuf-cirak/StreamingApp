import { Component, computed, inject } from '@angular/core';
import { RecommendedStreamersFacade } from './services/recommended-streamer.facade';
import { StreamerListComponent } from './components/list/streamer-list.component';
import { StreamerSkeletonComponent } from './components/skeleton/streamer-skeleton.component';

@Component({
  templateUrl: './recommended-streamers.component.html',
  standalone: true,
  selector: 'app-recommended-streamers',
  imports: [StreamerListComponent, StreamerSkeletonComponent],
  providers: [RecommendedStreamersFacade],
})
export class RecommendedStreamersComponent {
  readonly streamerFacade = inject(RecommendedStreamersFacade);

  liveStreamers = computed(
    () => this.streamerFacade.recommendedStreamers()?.liveStreamers
  );

  followingStreamers = computed(
    () => this.streamerFacade.recommendedStreamers()?.followingStreamers
  );
}

import { Component, inject } from '@angular/core';
import { RecommendedStreamersFacade } from './services/recommended-streamer.facade';
import { StreamerListComponent } from './components/list/streamer-list.component';
import { StreamerSkeletonComponent } from './components/skeleton/streamer-skeleton.component';

@Component({
  templateUrl: './recommended-streamers.component.html',
  standalone: true,
  selector: 'app-recommended-streamers',
  imports: [StreamerListComponent, StreamerSkeletonComponent],
})
export class RecommendedStreamersComponent {
  readonly streamerFacade = inject(RecommendedStreamersFacade);
}

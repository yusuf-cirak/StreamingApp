import { Component, inject } from '@angular/core';
import { StreamerFacade } from '../services/streamer.facade';
import { StreamerListComponent } from './list/streamer-list.component';
import { StreamerSkeletonComponent } from './skeleton/streamer-skeleton.component';

@Component({
  templateUrl: './streamers.component.html',
  standalone: true,
  selector: 'app-streamers',
  imports: [StreamerListComponent, StreamerSkeletonComponent],
})
export class StreamersComponent {
  readonly streamerFacade = inject(StreamerFacade);
}

import { Component, inject } from '@angular/core';
import { StreamerFacade } from '../services/streamer.facade';
import { StreamerListComponent } from './list/streamer-list.component';
import { StreamerSkeletonComponent } from './skeleton/streamer-skeleton.component';
import { Streamer } from '../models/streamer';

@Component({
  templateUrl: './streamers.component.html',
  standalone: true,
  selector: 'app-streamers',
  imports: [StreamerListComponent, StreamerSkeletonComponent],
})
export class StreamersComponent {
  readonly streamerFacade = inject(StreamerFacade);
  streamers: Streamer[] = [
    {
      id: '1',
      imageUrl: '1',
      isLive: true,
      streamId: '1',
      username: 'masumulu',
    },
  ];
}

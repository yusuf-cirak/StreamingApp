import { Component, inject } from '@angular/core';
import { StreamersFacade } from './services/streamers.facade';
import { StreamersListComponent } from './components/streamers-list/streamers-list.component';
import { StreamerSkeletonComponent } from './components/streamer-skeleton/streamer-skeleton.component';
import { AuthService } from '@streaming-app/core';

@Component({
  selector: 'app-streamers',
  standalone: true,
  imports: [StreamersListComponent, StreamerSkeletonComponent],
  templateUrl: './streamers.component.html',
})
export class StreamersComponent {
  readonly streamersFacade = inject(StreamersFacade);
  readonly authService = inject(AuthService);
}

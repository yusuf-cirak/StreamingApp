import { Component, computed, inject, signal } from '@angular/core';
import { StreamersFacade } from '../../../../modules/streamers/services/streamers.facade';
import { StreamerShowCardComponent } from './streamer-show-card/streamer-show-card.component';
import { StreamerShowCaseSkeletonComponent } from './streamer-show-card/streamer-show-case-skeleton/streamer-show-case-skeleton.component';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-streamers-show-case',
  standalone: true,
  imports: [
    StreamerShowCardComponent,
    StreamerShowCaseSkeletonComponent,
    NgTemplateOutlet,
  ],
  templateUrl: './streamers-show-case.component.html',
})
export class StreamersShowCaseComponent {
  readonly streamersFacade = inject(StreamersFacade);

  readonly allStreamers = computed(
    () => this.streamersFacade.streamers()?.allStreamers || []
  );
}

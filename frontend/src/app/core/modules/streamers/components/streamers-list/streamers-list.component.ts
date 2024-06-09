import { Component, input } from '@angular/core';
import { StreamerComponent } from '../streamer/streamer.component';
import { StreamDto } from '../../../streams/contracts/stream-dto';

@Component({
  selector: 'app-streamers-list',
  standalone: true,
  imports: [StreamerComponent],
  templateUrl: './streamers-list.component.html',
})
export class StreamersListComponent {
  readonly streamers = input.required<StreamDto[]>();
}

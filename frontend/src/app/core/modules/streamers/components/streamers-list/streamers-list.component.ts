import { Component, input } from '@angular/core';
import { StreamerDto } from '../../models/streamer-dto';
import { StreamerComponent } from '../streamer/streamer.component';

@Component({
  selector: 'app-streamers-list',
  standalone: true,
  imports: [StreamerComponent],
  templateUrl: './streamers-list.component.html',
})
export class StreamersListComponent {
  readonly streamers = input.required<StreamerDto[]>();
}

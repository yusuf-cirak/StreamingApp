import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { StreamDto } from '../../../../../modules/streams/contracts/stream-dto';
import { ThumbnailComponent } from '../../../../../components/thumbnail/thumbnail.component';

@Component({
  selector: 'app-streamer-show-card',
  standalone: true,
  imports: [RouterLink, ThumbnailComponent],
  templateUrl: './streamer-show-card.component.html',
})
export class StreamerShowCardComponent {
  streamer = input.required<StreamDto>();
}

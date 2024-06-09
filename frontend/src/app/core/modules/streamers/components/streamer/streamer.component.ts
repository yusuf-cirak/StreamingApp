import { Component, computed, inject, input } from '@angular/core';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { RouterLink } from '@angular/router';
import { NgTemplateOutlet } from '@angular/common';
import { ImageService } from '../../../../services/image.service';

@Component({
  selector: 'app-streamer',
  standalone: true,
  imports: [RouterLink, NgTemplateOutlet],
  templateUrl: './streamer.component.html',
})
export class StreamerComponent {
  readonly streamer = input.required<StreamDto>();

  readonly imageService = inject(ImageService);

  isLive = computed(() => {
    const stream = this.streamer() as StreamDto;
    return !!stream?.streamOption?.streamKey;
  });

  getUser = computed(() => {
    const streamer = this.streamer();

    return streamer?.user || streamer;
  });
}

import { Component, computed, inject, input } from '@angular/core';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { RouterLink } from '@angular/router';
import { StreamerDto } from '../../models/streamer-dto';
import { NgTemplateOutlet } from '@angular/common';
import { ImageService } from '../../../../services/image.service';

@Component({
  selector: 'app-streamer',
  standalone: true,
  imports: [RouterLink, NgTemplateOutlet],
  templateUrl: './streamer.component.html',
})
export class StreamerComponent {
  readonly user = input.required<StreamerDto>();

  readonly imageService = inject(ImageService);

  isLive = computed(() => {
    return (this.user() as StreamDto)?.streamOption?.streamKey !== undefined;
  });
}

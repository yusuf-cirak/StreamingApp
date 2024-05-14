import { Component, computed, inject, input } from '@angular/core';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { RouterLink } from '@angular/router';
import { UserImageService } from '../../../../services/user-image.service';
import { StreamerDto } from '../../models/streamer-dto';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-streamer',
  standalone: true,
  imports: [RouterLink, NgTemplateOutlet],
  templateUrl: './streamer.component.html',
})
export class StreamerComponent {
  readonly user = input.required<StreamerDto>();

  readonly userImageService = inject(UserImageService);

  isLive = computed(() => {
    return (this.user() as StreamDto)?.streamOption?.streamKey !== undefined;
  });
}

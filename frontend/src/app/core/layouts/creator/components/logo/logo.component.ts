import { NgOptimizedImage } from '@angular/common';
import { Component, inject, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SpookyIcon } from '@streaming-app/shared/icons';
import { CreatorService } from '../../services/creator-service';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, of } from 'rxjs';
import { LogoSkeletonComponent } from './logo-skeleton.component';

@Component({
  selector: 'app-creator-logo',
  standalone: true,
  imports: [NgOptimizedImage, RouterLink, SpookyIcon, LogoSkeletonComponent],
  encapsulation: ViewEncapsulation.None,
  template: `
    @defer (when streamerName()) {
    <div
      class="flex items-start gap-x-4 hover:opacity-75 transition hover:cursor-pointer"
      [routerLink]="['', 'creator', streamerName()]"
    >
      <app-spooky-icon
        class="bg-white rounded-full p-1 lg:mr-0 lg:shrink shrink-0 w-12 h-12"
      />

      <div class="hidden lg:block flex-col items-start">
        <p class="text-lg text-white font-semibold">Streaming App</p>
        <p class="text-sm text-muted-foreground">
          {{ streamerName() }}'s Dashboard
        </p>
      </div>
    </div>

    } @placeholder {
    <app-logo-skeleton></app-logo-skeleton>
    }
  `,
})
export class CreatorLogoComponent {
  readonly creatorService = inject(CreatorService);

  readonly streamerName = this.creatorService.streamerName;
}

import { NgOptimizedImage } from '@angular/common';
import { Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SpookyIcon } from '@streaming-app/shared/icons';

@Component({
  selector: 'app-creator-logo',
  standalone: true,
  imports: [NgOptimizedImage, RouterLink, SpookyIcon],
  encapsulation: ViewEncapsulation.None,
  template: `
    <div
      class="flex items-start gap-x-4 hover:opacity-75 transition hover:cursor-pointer"
      [routerLink]="['/']"
    >
      <app-spooky-icon
        class="bg-white rounded-full p-1 lg:mr-0 lg:shrink shrink-0 w-12 h-12"
      />

      <div class="hidden lg:block flex-col items-start">
        <p class="text-lg text-white font-semibold">Streaming App</p>
        <p class="text-sm text-muted-foreground">Creator Dashboard</p>
      </div>
    </div>
  `,
})
export class CreatorLogoComponent {}

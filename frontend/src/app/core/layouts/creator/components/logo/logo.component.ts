import { NgOptimizedImage } from '@angular/common';
import { Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-creator-logo',
  standalone: true,
  imports: [NgOptimizedImage, RouterLink],
  encapsulation: ViewEncapsulation.None,
  template: `
    <div
      class="flex items-start gap-x-4 hover:opacity-75 transition hover:cursor-pointer"
      [routerLink]="['/', 'creator']"
    >
      <div class="bg-white rounded-full p-1 lg:mr-0 lg:shrink shrink-0">
        <img
          ngSrc="../../../../../../assets/spooky.svg"
          [width]="32"
          [height]="32"
        />
      </div>
      <div class="hidden lg:block flex-col items-start">
        <p class="text-lg text-white font-semibold">Streaming App</p>
        <p class="text-sm text-muted-foreground">Creator Dashboard</p>
      </div>
    </div>
  `,
})
export class CreatorLogoComponent {}

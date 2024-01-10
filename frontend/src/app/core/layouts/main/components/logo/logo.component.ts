import { NgOptimizedImage } from '@angular/common';
import { Component, ViewEncapsulation } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-logo',
  standalone: true,
  imports: [NgOptimizedImage, RouterLink],
  encapsulation: ViewEncapsulation.None,
  template: `
    <div
      class="flex items-start gap-x-4 hover:opacity-75 transition hover:cursor-pointer"
      [routerLink]="['/']"
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
        <p class="text-sm text-muted-foreground">Let&apos;s play</p>
      </div>
    </div>
  `,
})
export class LogoComponent {}

import { Component } from '@angular/core';

@Component({
  selector: 'app-logo-skeleton',
  standalone: true,
  template: `
    <div
      class="flex items-start gap-x-4 hover:opacity-75 transition animate-pulse"
    >
      <div
        class="bg-white rounded-full p-1 lg:mr-0 lg:shrink shrink-0 w-12 h-12"
      ></div>
      <div class="hidden lg:block flex-col items-start">
        <div class="h-4 w-24 bg-gray-200 rounded"></div>
        <div class="h-3 w-32 mt-1 bg-gray-200 rounded"></div>
      </div>
    </div>
  `,
})
export class LogoSkeletonComponent {}

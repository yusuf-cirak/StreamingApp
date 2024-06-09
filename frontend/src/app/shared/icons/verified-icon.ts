import { Component } from '@angular/core';

@Component({
  selector: 'app-verified-icon',
  standalone: true,
  template: `
    <div
      class="p-0.5 flex items-center justify-center h-4 w-4 rounded-full bg-blue-600"
    >
      <svg
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        stroke-width="1.5"
        stroke="currentColor"
        class="w-6 h-6 text-primary-stroke-[4px]"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="m4.5 12.75 6 6 9-13.5"
        />
      </svg>
    </div>
  `,
})
export class VerifiedIcon {}

import { Component } from '@angular/core';

@Component({
  selector: 'app-block-icon',
  standalone: true,
  template: ` <svg
    version="1.1"
    viewBox="0 0 16 16"
    xmlns="http://www.w3.org/2000/svg"
    xmlns:xlink="http://www.w3.org/1999/xlink"
    class="w-6 h-6"
    fill="white"
  >
    <path
      d="M11.4286,0h-6.85718l-4.57141,4.57141v6.85712l4.57141,4.57147h6.85718l4.57141,-4.57147v-6.85712l-4.57141,-4.57141Zm1.57141,9h-10v-2h10v2Z"
    ></path>
  </svg>`,
})
export class BlockIcon {}

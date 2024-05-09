import { Component } from '@angular/core';

@Component({
  selector: 'app-chat-disabled-icon',
  standalone: true,
  template: `<svg
    class="w-24 h-24"
    viewBox="0 0 24 24"
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    xmlns:xlink="http://www.w3.org/1999/xlink"
  >
    <g
      id="web-app"
      stroke="none"
      stroke-width="1"
      fill="none"
      fill-rule="evenodd"
    >
      <g id="disabled" fill="currentColor">
        <path
          d="M12,22 C17.5228475,22 22,17.5228475 22,12 C22,6.4771525 17.5228475,2 12,2 C6.4771525,2 2,6.4771525 2,12 C2,17.5228475 6.4771525,22 12,22 Z M7.09435615,18.3198574 L18.3198574,7.09435615 C19.3729184,8.44903985 20,10.1512885 20,12 C20,16.418278 16.418278,20 12,20 C10.1512885,20 8.44903985,19.3729184 7.09435615,18.3198574 Z M5.68014258,16.9056439 C4.62708161,15.5509601 4,13.8487115 4,12 C4,7.581722 7.581722,4 12,4 C13.8487115,4 15.5509601,4.62708161 16.9056439,5.68014258 L5.68014258,16.9056439 Z"
          id="Shape"
        ></path>
      </g>
    </g>
  </svg>`,
})
export class ChatDisabledIcon {}

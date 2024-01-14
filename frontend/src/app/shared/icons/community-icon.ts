import { Component } from '@angular/core';

@Component({
  selector: 'app-community-icon',
  standalone: true,
  template: ` <svg
    viewBox="0 0 24 24"
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    xmlns:xlink="http://www.w3.org/1999/xlink"
    class="w-8 h-8"
    fill="currentColor"
  >
    <title>User</title>
    <g
      id="User"
      stroke="none"
      stroke-width="1"
      fill="none"
      fill-rule="evenodd"
      stroke-dasharray="0,0"
      stroke-linecap="round"
    >
      <path
        d="M12,11 C13.3807119,11 14.5,9.88071187 14.5,8.5 C14.5,7.11928813 13.3807119,6 12,6 C10.6192881,6 9.5,7.11928813 9.5,8.5 C9.5,9.88071187 10.6192881,11 12,11 Z"
        id="shape-02"
        stroke="currentColor"
        stroke-width="2"
      ></path>
      <path
        d="M6,20 C6,16.6862915 8.6862915,14 12,14 C15.3137085,14 18,16.6862915 18,20 L18,20 L18,20"
        id="Path"
        stroke="currentColor"
        stroke-width="2"
      ></path>
    </g>
  </svg>`,
})
export class CommunityIconComponent {}

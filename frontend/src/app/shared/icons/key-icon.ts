import { Component } from '@angular/core';

@Component({
  selector: 'app-key-icon',
  standalone: true,
  template: `<svg
    class="w-8 h-8"
    fill="currentColor"
    viewBox="0 0 24 24"
    version="1.1"
    xmlns="http://www.w3.org/2000/svg"
    xmlns:xlink="http://www.w3.org/1999/xlink"
  >
    <title>Key</title>
    <g id="Key" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
      <rect id="Container" x="0" y="0" width="24" height="24"></rect>
      <path
        d="M8.5,20 C10.9852814,20 13,17.9852814 13,15.5 C13,13.0147186 10.9852814,11 8.5,11 C6.01471863,11 4,13.0147186 4,15.5 C4,17.9852814 6.01471863,20 8.5,20 Z"
        id="shape-1"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-dasharray="0,0"
      ></path>
      <path
        d="M12,12 L20,4"
        id="shape-2"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-dasharray="0,0"
      ></path>
      <path
        d="M17,6 L18,6 C19.1045695,6 20,6.8954305 20,8 C20,9.1045695 19.1045695,10 18,10 L17,10"
        id="shape-3"
        stroke="currentColor"
        stroke-width="2"
        stroke-linecap="round"
        stroke-dasharray="0,0"
        transform="translate(18.500000, 8.000000) rotate(45.000000) translate(-18.500000, -8.000000) "
      ></path>
    </g>
  </svg>`,
})
export class KeyIconComponent {}

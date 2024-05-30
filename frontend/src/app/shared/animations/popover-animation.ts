import { trigger, transition, style, animate } from '@angular/animations';

export const popoverAnimation = trigger('popoverAnimation', [
  transition(':enter', [
    style({ opacity: 0 }), // From
    animate('200ms ease-in', style({ opacity: 1 })), // To
  ]),
  transition(':leave', [
    style({ opacity: 1 }), // From
    animate('100ms ease-out', style({ opacity: 0 })), // To
  ]),
]);

import { trigger, transition, style, animate } from '@angular/animations';

export const fadeAnimation = trigger('fadeAnimation', [
  transition(':enter', [
    style({ opacity: 0 }),
    animate('150ms ease-in-out', style({ opacity: 1 })),
  ]),
  transition(':leave', [animate('150ms ease-in-out', style({ opacity: 0 }))]),
]);

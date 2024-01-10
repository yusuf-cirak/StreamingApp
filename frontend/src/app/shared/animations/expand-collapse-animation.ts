import { trigger, transition, style, animate } from '@angular/animations';

export const expandCollapseAnimation = trigger('expandCollapseAnimation', [
  transition(':enter', [
    style({ opacity: 0.25, maxHeight: '0' }),
    animate('300ms ease-in-out', style({ opacity: 1, maxHeight: 'none' })),
  ]),
  transition(':leave', [
    animate('300ms ease-in-out', style({ opacity: 0, maxHeight: '0' })),
  ]),
]);

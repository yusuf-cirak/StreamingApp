import { trigger, style, animate, transition } from '@angular/animations';

export const textClipAnimation = trigger('textClipAnimation', [
  transition(':enter', [
    style({
      textTransform: 'uppercase',
      background:
        'linear-gradient(-225deg, #231557 0%, #44107a 29%, #ff1361 67%, #fff800 100%)',
      color: 'transparent', // Initially transparent
      backgroundSize: '200% auto',
      backgroundClip: 'text',
      textFillColor: 'transparent',
      WebkitBackgroundClip: 'text',
      WebkitTextFillColor: 'transparent',
      backgroundPosition: 'left center', // Start from the left (-200%)
    }),
    animate(
      '1s linear',
      style({
        color: '#fff', // Make text opaque
        backgroundPosition: 'right center', // End at the right (200%)
      })
    ),
  ]),
]);

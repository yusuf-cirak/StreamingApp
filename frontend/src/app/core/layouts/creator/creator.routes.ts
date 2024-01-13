import { Route } from '@angular/router';

export const creatorRoutes: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [],
    loadComponent: () =>
      import('@streaming-app/layouts/creator').then((m) => m.CreatorLayout),
  },
];

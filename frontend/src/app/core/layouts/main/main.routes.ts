import { Route } from '@angular/router';

export const mainRoutes: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [],
    loadComponent: () =>
      import('@streaming-app/layouts/main').then((m) => m.MainLayout),
  },
];

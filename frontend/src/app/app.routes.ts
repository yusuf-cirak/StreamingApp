import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'creator',
    loadChildren: () =>
      import('@streaming-app/layouts/creator').then((m) => m.creatorRoutes),
  },
  {
    path: '',
    loadChildren: () =>
      import('@streaming-app/layouts/main').then((m) => m.mainRoutes),
  },
  {
    path: '**',
    loadComponent: () =>
      import('./core/components/not-found/not-found.component').then(
        (m) => m.NotFoundComponent
      ),
  },
];

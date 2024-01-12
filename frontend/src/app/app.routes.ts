import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('@streaming-app/layouts/main').then((m) => m.mainRoutes),
  },
  {
    path: 'not-found',
    loadComponent: () =>
      import('./core/components/not-found/not-found.component').then(
        (m) => m.NotFoundComponent
      ),
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];

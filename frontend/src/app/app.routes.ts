import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./core/layouts/main/main.layout').then((m) => m.MainLayout),
  },
];

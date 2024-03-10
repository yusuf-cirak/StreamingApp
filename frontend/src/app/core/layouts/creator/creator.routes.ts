import { Route } from '@angular/router';
import { authGuard } from '../../guards/auth.guard';

export const creatorRoutes: Route[] = [
  {
    path: '',
    pathMatch: 'prefix',
    canActivate: [],
    loadComponent: () =>
      import('@streaming-app/layouts/creator').then((m) => m.CreatorLayout),
    children: [
      {
        path: 'chat-settings',
        pathMatch: 'full',
        canActivate: [],
        loadComponent: () =>
          import('./components/chat-settings/chat-settings.component').then(
            (c) => c.ChatSettingsComponent
          ),
      },
      {
        path: 'key',
        pathMatch: 'full',
        canActivateChild: [authGuard],
        loadComponent: () =>
          import('../../modules/key/key.component').then((c) => c.KeyComponent),
      },
    ],
  },
];

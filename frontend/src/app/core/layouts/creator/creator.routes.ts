import { Route } from '@angular/router';

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
        canActivate: [],
        loadComponent: () =>
          import('./components/key/key.component').then((c) => c.KeyComponent),
      },
    ],
  },
];

import { Route } from '@angular/router';
import { authGuard } from '../../guards/auth.guard';
import { streamStateResolver } from '../../modules/streams/resolvers/stream-state.resolver';

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
        canActivate: [authGuard],
        loadComponent: () =>
          import(
            '../../modules/stream-options/components/chat-settings/chat-settings.component'
          ).then((c) => c.ChatSettingsComponent),
      },
      {
        path: 'key',
        pathMatch: 'full',
        canActivateChild: [authGuard],
        loadComponent: () =>
          import('../../modules/key/key.component').then((c) => c.KeyComponent),
      },
      {
        path: 'stream',
        pathMatch: 'full',
        canActivateChild: [authGuard],
        resolve: {
          streamState: streamStateResolver,
        },
        loadComponent: () =>
          import('../../modules/streams/stream.component').then(
            (c) => c.StreamComponent
          ),
      },
    ],
  },
];

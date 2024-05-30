import { Route } from '@angular/router';
import { authGuard } from '../../guards/auth.guard';
import { streamStateResolver } from '../../modules/streams/resolvers/stream-state.resolver';
import {
  chatSettingsGuard,
  communitySettingsGuard,
  creatorGuard,
  keySettingsGuard,
} from './guards/creator-page.guard';

export const creatorRoutes: Route[] = [
  {
    path: ':streamerName',
    pathMatch: 'prefix',
    canActivate: [authGuard, creatorGuard],

    loadComponent: () =>
      import('@streaming-app/layouts/creator').then((m) => m.CreatorLayout),
    children: [
      {
        path: 'stream',
        pathMatch: 'full',
        resolve: {
          // todo: remove this. don't remove the state in stream.facade. this is why you have to pass this again.
          streamState: streamStateResolver,
        },
        loadComponent: () =>
          import('../../modules/streams/stream.component').then(
            (c) => c.StreamComponent
          ),
      },
      {
        path: 'chat-settings',
        pathMatch: 'full',
        canActivate: [chatSettingsGuard],
        loadComponent: () =>
          import(
            '../../modules/stream-options/components/chat-settings/chat-settings.component'
          ).then((c) => c.ChatSettingsComponent),
      },
      {
        path: 'key',
        pathMatch: 'full',
        canActivate: [keySettingsGuard],
        loadComponent: () =>
          import('../../modules/key/key.component').then((c) => c.KeyComponent),
      },
      {
        path: 'community',
        pathMatch: 'full',
        canActivate: [communitySettingsGuard],
        loadComponent: () =>
          import(
            '../../modules/community-settings/community-settings.component'
          ).then((c) => c.CommunitySettingsComponent),
      },
    ],
  },
];

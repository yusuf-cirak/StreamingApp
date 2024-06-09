import { Route } from '@angular/router';
import { authGuard } from '../../guards/auth.guard';
import {
  creatorLayoutGuard,
  creatorPageGuard,
} from './guards/creator-page.guard';

export const creatorRoutes: Route[] = [
  {
    path: ':streamerName',
    pathMatch: 'prefix',
    canActivate: [authGuard, creatorLayoutGuard],

    loadComponent: () =>
      import('@streaming-app/layouts/creator').then((m) => m.CreatorLayout),
    children: [
      {
        path: 'stream',
        pathMatch: 'full',
        loadComponent: () =>
          import('../../modules/streams/stream.component').then(
            (c) => c.StreamComponent
          ),
      },
      {
        path: 'chat-settings',
        pathMatch: 'full',
        canActivate: [creatorPageGuard('chat-settings')],
        loadComponent: () =>
          import(
            '../../modules/stream-options/components/chat-settings/chat-settings.component'
          ).then((c) => c.ChatSettingsComponent),
      },
      {
        path: 'key',
        pathMatch: 'full',
        canActivate: [creatorPageGuard('key')],
        loadComponent: () =>
          import('../../modules/key/key.component').then((c) => c.KeyComponent),
      },
      {
        path: 'community',
        pathMatch: 'full',
        canActivate: [creatorPageGuard('community')],
        loadComponent: () =>
          import(
            '../../modules/community-settings/community-settings.component'
          ).then((c) => c.CommunitySettingsComponent),
      },

      {
        path: 'moderators',
        pathMatch: 'full',
        canActivate: [creatorPageGuard('moderators')],
        loadComponent: () =>
          import(
            '../../modules/moderator-settings/moderator-settings.component'
          ).then((c) => c.ModeratorSettingsComponent),
      },
    ],
  },
];

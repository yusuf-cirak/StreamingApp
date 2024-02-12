import { Route } from '@angular/router';
import { streamStateResolver } from '../../modules/streams/resolvers/stream-state.resolver';

export const mainRoutes: Route[] = [
  {
    path: '',
    pathMatch: 'full',
    canActivate: [],
    loadComponent: () =>
      import('@streaming-app/layouts/main').then((m) => m.MainLayout),
  },
  {
    path: ':streamerName',
    pathMatch: 'full',
    canActivate: [],
    resolve: {
      streamState: streamStateResolver,
    },
    loadComponent: () =>
      import('@streaming-app/layouts/main').then((m) => m.MainLayout),
  },
];

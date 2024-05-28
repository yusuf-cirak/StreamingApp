import { switchMap, tap, of, forkJoin, concatMap } from 'rxjs';
import { AuthService } from '..';
import { StreamHub } from '../hubs/stream-hub';
import { APP_INITIALIZER, Provider } from '@angular/core';
import { StreamProxyService } from '../modules/streams/services/stream-proxy.service';
export const INITIALIZE_USER_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializeUserFactory,
  deps: [AuthService, StreamHub, StreamProxyService],
  multi: true,
};

function initializeUserFactory(
  authService: AuthService,
  streamHub: StreamHub,
  streamProxyService: StreamProxyService
) {
  return () => {
    const followingAndBlocked$ = location.pathname.includes('/creator')
      ? forkJoin([
          streamProxyService.getBlocked(),
          streamProxyService.getFollowing(),
        ]).pipe(
          tap(([blocked, following]) => {
            authService.updateBlockedStreamers(blocked.map((b) => b.user.id));
            authService.updateFollowingStreamers(
              following.map((f) => f.user.id)
            );
          })
        )
      : of(null);

    return authService.initializeUser().pipe(
      tap(() => console.log('user is initialized')),
      concatMap(() =>
        !streamHub.connectedToHub()
          ? streamHub.buildAndConnect(authService.user()?.accessToken)
          : of(null)
      ),
      concatMap(() => followingAndBlocked$)
    );
  };
}

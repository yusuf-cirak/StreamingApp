import { switchMap, tap, of, forkJoin, concatMap, catchError } from 'rxjs';
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
    const null$ = of(null);

    authService
      .initializeUser()
      .pipe(
        tap(() => console.log('user is initialized')),
        concatMap(() =>
          !streamHub.connectedToHub()
            ? streamHub
                .buildAndConnect(authService.user()?.accessToken)
                .pipe(catchError(() => null$))
            : null$
        ),
        concatMap(() =>
          location.pathname.includes('/creator') &&
          authService.isAuthenticated()
            ? forkJoin([
                streamProxyService.getBlocked(),
                streamProxyService.getFollowing(),
              ]).pipe(
                tap(([blocked, following]) => {
                  authService.updateBlockedStreamers(
                    blocked.map((b) => b.user.id)
                  );
                  authService.updateFollowingStreamers(
                    following.map((f) => f.user.id)
                  );
                })
              )
            : null$
        )
      )
      .subscribe();

    return Promise.resolve();
  };
}

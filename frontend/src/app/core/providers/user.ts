import { from, catchError, of, forkJoin, EMPTY, switchMap } from 'rxjs';
import { AuthService } from '..';
import { StreamHub } from '../hubs/stream-hub';
import { APP_INITIALIZER, Provider } from '@angular/core';
import { UserService } from '../modules/users/services/user-service';

export const INITIALIZE_USER_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializeUserFactory,
  deps: [AuthService, StreamHub, UserService],
  multi: true,
};

function initializeUserFactory(
  authService: AuthService,
  streamHub: StreamHub,
  userService: UserService
) {
  return () => {
    const location = window.location.pathname;
    const isCreatorRoute = location.includes('/creator');
    var initializer$ = from(authService.initializeUser()).pipe(
      catchError((error) => {
        console.error('Error initializing user', error);
        return EMPTY;
      }),
      switchMap(() => {
        const isAuthenticated = authService.isAuthenticated();
        return forkJoin([
          from(streamHub.buildAndConnect(authService.user()?.accessToken)),
          ...(isAuthenticated
            ? [
                userService.getBlockedStreamers(),
                isCreatorRoute ? userService.getFollowingStreamers() : of([]),
              ]
            : []),
        ]);
      })
    );

    initializer$.subscribe();
    return Promise.resolve();
  };
}

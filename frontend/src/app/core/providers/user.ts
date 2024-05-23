import { from, tap, catchError, of, concatMap, forkJoin } from 'rxjs';
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
    return from(authService.initializeUser()).pipe(
      tap(() => console.log('User initialized')),
      concatMap(() =>
        forkJoin([
          from(streamHub.buildAndConnect(authService.user()?.accessToken)),
          userService.getBlockedStreamers(),
        ])
      ),
      catchError((error) => {
        console.error('Error initializing user', error);
        return of(null);
      })
    );
  };
}

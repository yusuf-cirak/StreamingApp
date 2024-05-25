import { from, catchError, EMPTY, switchMap, tap } from 'rxjs';
import { AuthService } from '..';
import { StreamHub } from '../hubs/stream-hub';
import { APP_INITIALIZER, Provider } from '@angular/core';
export const INITIALIZE_USER_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializeUserFactory,
  deps: [AuthService, StreamHub],
  multi: true,
};

function initializeUserFactory(authService: AuthService, streamHub: StreamHub) {
  return () => {
    const initializer$ = from(authService.initializeUser()).pipe(
      catchError((error) => {
        console.error('Error initializing user', error);
        return EMPTY;
      }),
      tap(() => console.log('user is initialized')),
      switchMap(() =>
        streamHub.buildAndConnect(authService.user()?.accessToken)
      )
    );

    return initializer$;
  };
}

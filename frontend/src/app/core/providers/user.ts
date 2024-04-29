import { from, tap, switchMap } from 'rxjs';
import { AuthService } from '..';
import { StreamHub } from '../hubs/stream-hub';
import { APP_INITIALIZER, Provider } from '@angular/core';

function initializeUserFactory(authService: AuthService, streamHub: StreamHub) {
  return () => {
    return from(authService.initializeUser()).pipe(
      tap(() => console.log('User initialized')),
      switchMap(() => streamHub.connect())
    );
  };
}

export const INITIALIZE_USER_PROVIDER: Provider = {
  provide: APP_INITIALIZER,
  useFactory: initializeUserFactory,
  deps: [AuthService, StreamHub],
  multi: true,
};

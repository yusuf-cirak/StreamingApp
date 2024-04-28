import {
  APP_INITIALIZER,
  ApplicationConfig,
  importProvidersFrom,
} from '@angular/core';
import {
  provideRouter,
  withComponentInputBinding,
  withViewTransitions,
} from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  provideHttpClient,
  withFetch,
  withInterceptors,
} from '@angular/common/http';
import { authInterceptor, AuthService } from './core';
import { StreamHub } from './core/hubs/stream-hub';
import {  from, switchMap, tap } from 'rxjs';

function initializeApp(authService: AuthService, streamHub: StreamHub) {
  return () => {
    return from(authService.initializeUser()).pipe(
      tap(() => console.log('User initialized')),
      switchMap(() => streamHub.connect())
    );
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withViewTransitions()),
    // provideClientHydration(),
    provideAnimations(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    provideToastr({
      timeOut: 5000,
      preventDuplicates: true,
      easing: 'ease-in',
      easeTime: 300,
    }),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [AuthService, StreamHub],
      multi: true,
    },
  ],
};

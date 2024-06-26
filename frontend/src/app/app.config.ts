import { ApplicationConfig } from '@angular/core';
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
import { authInterceptor } from './core';
import { INITIALIZE_USER_PROVIDER } from './core/providers/user';
import { PRIMENG_CONFIG_PROVIDER } from './core/config/primeng';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withViewTransitions()),
    provideAnimations(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    provideToastr({
      timeOut: 5000,
      preventDuplicates: true,
      easing: 'ease-in',
      easeTime: 300,
      positionClass: 'toast-bottom-center',
    }),
    PRIMENG_CONFIG_PROVIDER,
    INITIALIZE_USER_PROVIDER,
  ],
};

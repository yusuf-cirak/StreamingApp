import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AuthService } from '../services';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);

  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  }

  return authService.refreshToken().pipe(
    map(() => {
      return true;
    }),
    catchError((_) => {
      router.navigateByUrl('/');
      return of(false);
    })
  );
};

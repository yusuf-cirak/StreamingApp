import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AuthService } from '../services';

export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);

  if (authService.isAuthenticated()) {
    return true;
  }

  const router = inject(Router);

  return authService.refreshToken().pipe(
    map((user) => {
      const { claims, ...rest } = user;
      authService.setUser({
        ...rest,
        roles: claims.roles,
        operationClaims: claims.operationClaims,
      });

      return true;
    }),
    catchError((_) => {
      router.navigateByUrl('/');
      return of(false);
    })
  );
};

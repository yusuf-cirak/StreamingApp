import { inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse,
} from '@angular/common/http';
import { catchError, tap, throwError, mergeMap } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../services';

export function authInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
) {
  console.log('authInterceptor');
  const skipInterceptor = request.headers.get('SkipInterceptor');
  request.headers.delete('SkipInterceptor');

  if (skipInterceptor === 'true') {
    return next(request);
  }
  const authService = inject(AuthService);

  if (!authService.isAuthenticated()) {
    return next(request);
  }

  const router = inject(Router);

  const accessToken = authService.user()?.accessToken;

  const newRequest = request.clone({
    headers: request.headers.set('Authorization', `Bearer ${accessToken}`),
  });

  return next(newRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return authService.refreshToken().pipe(
          catchError((_) => {
            debugger;
            router.navigateByUrl('');
            return throwError(() => error);
          }),
          tap((user) => {
            const { claims, ...rest } = user;
            authService.setUser({
              ...rest,
              roles: claims.roles,
              operationClaims: claims.operationClaims,
            });
          }),
          mergeMap((user) => {
            const newRequest = request.clone({
              headers: request.headers.set(
                'Authorization',
                `Bearer ${user.accessToken}`
              ),
            });
            return next(newRequest);
          })
        );
      }
      return throwError(() => error);
    })
  );
}

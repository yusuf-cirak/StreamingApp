import { Injectable, computed, inject, signal } from '@angular/core';
import { CurrentUser } from '../../../models/current-user';
import { AuthProxyService } from './auth-proxy.service';
import { UserLoginDto } from '../../../dtos/user-login-dto';
import { UserRegisterDto } from '../../../dtos/user-register-dto';
import { UserRefreshTokenDto } from '../../../dtos/user-refresh-token-dto';
import { UserAuthDto } from '../dtos/user-auth-dto';
import { lastValueFrom, tap, throwError } from 'rxjs';
import { LocalStorageEventService } from '@streaming-app/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly localStorageEventService = inject(LocalStorageEventService);

  readonly authProxyService = inject(AuthProxyService);

  #user = signal<CurrentUser | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  readonly userId = computed(() => this.user()?.id);

  readonly isAuthenticated = computed(() => {
    const user = this.user();

    return !!user && user.tokenExpiration > new Date(Date.now());
  });

  constructor() {
    this.handleStorageEvents();
  }

  getUserFromLocalStorage(): UserAuthDto | null {
    const user = localStorage.getItem('user');
    return user?.length ? JSON.parse(user) : null;
  }

  mapToCurrentUser(userAuthDto: UserAuthDto): CurrentUser {
    const { claims, ...rest } = userAuthDto;

    rest.tokenExpiration = new Date(rest.tokenExpiration);

    return {
      ...rest,
      roles: claims.roles,
      operationClaims: claims.operationClaims,
    };
  }

  async initializeUser() {
    const userAuthDto = this.getUserFromLocalStorage();

    if (!userAuthDto) {
      return;
    }

    const tokenExpiration = new Date(userAuthDto.tokenExpiration);

    userAuthDto.tokenExpiration = tokenExpiration;

    this.setUser(userAuthDto);

    if (tokenExpiration < new Date(Date.now())) {
      await lastValueFrom(
        this.refreshToken(this.mapToCurrentUser(userAuthDto))
      ).catch((err) => {});
    }
  }

  setUser(userAuthDto: UserAuthDto) {
    if (!userAuthDto) {
      this.#user.set(undefined);
      return;
    }

    const user = this.mapToCurrentUser(userAuthDto);
    this.#user.set(user);

    localStorage.setItem('user', JSON.stringify(userAuthDto));
  }

  login(credentials: UserLoginDto) {
    return this.authProxyService
      .login(credentials)
      .pipe(tap((userAuthDto) => this.setUser(userAuthDto)));
  }

  register(credentials: UserRegisterDto) {
    return this.authProxyService
      .register(credentials)
      .pipe(tap((userAuthDto) => this.setUser(userAuthDto)));
  }

  refreshToken(user?: CurrentUser) {
    user = user ?? this.user();

    if (!user) {
      return throwError('User is not authenticated');
    }

    const userRefreshTokenDto: UserRefreshTokenDto = {
      refreshToken: user?.refreshToken!,
      userId: user?.id!,
    };

    return this.authProxyService
      .refreshToken(userRefreshTokenDto)
      .pipe(tap((userAuthDto) => this.setUser(userAuthDto)));
  }

  logout() {
    this.#user.set(undefined);
    localStorage.removeItem('user');
  }

  private handleStorageEvents() {
    this.localStorageEventService.userChanged$
      .pipe(takeUntilDestroyed())
      .subscribe({
        next: (value) => {
          const user = value ? JSON.parse(value) : undefined;
          this.setUser(user);
        },
      });
  }
}

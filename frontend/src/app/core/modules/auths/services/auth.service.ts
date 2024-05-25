import { Injectable, computed, inject, signal } from '@angular/core';
import { CurrentUser } from '../../../models/current-user';
import { AuthProxyService } from './auth-proxy.service';
import { UserLoginDto } from '../../../dtos/user-login-dto';
import { UserRegisterDto } from '../../../dtos/user-register-dto';
import { UserRefreshTokenDto } from '../../../dtos/user-refresh-token-dto';
import { UserAuthDto } from '../dtos/user-auth-dto';
import {
  EMPTY,
  lastValueFrom,
  map,
  Subject,
  switchMap,
  tap,
  throwError,
} from 'rxjs';
import { LocalStorageEventService, User } from '@streaming-app/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { StreamHub } from '../../../hubs/stream-hub';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly localStorageEventService = inject(LocalStorageEventService);

  readonly authProxyService = inject(AuthProxyService);

  readonly streamHub = inject(StreamHub);

  #user = signal<CurrentUser | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  #followingStreamIds = signal<string[]>([]);

  readonly followingStreamIds = this.#followingStreamIds.asReadonly();

  #blockedStreamIds = signal<string[]>([]);
  readonly blockedStreamIds = this.#blockedStreamIds.asReadonly();

  readonly login$ = new Subject<void>();

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

  currentUserToUserDto(): User {
    const current = this.user() as User;
    return {
      id: current.id,
      username: current.username,
      profileImageUrl: current.profileImageUrl,
    };
  }

  async initializeUser() {
    const userAuthDto = this.getUserFromLocalStorage();

    if (!userAuthDto) {
      return throwError('User is not authenticated');
    }

    const tokenExpiration = new Date(userAuthDto.tokenExpiration);

    userAuthDto.tokenExpiration = tokenExpiration;

    if (tokenExpiration < new Date(Date.now())) {
      return await lastValueFrom(
        this.refreshToken(this.mapToCurrentUser(userAuthDto))
      ).catch(() => localStorage.removeItem('user'));
    }

    this.setUser(userAuthDto);
    return EMPTY;
  }

  setUser(userAuthDto: UserAuthDto) {
    if (!userAuthDto) {
      this.#user.set(undefined);
      localStorage.removeItem('user');
      return;
    }

    const user = this.mapToCurrentUser(userAuthDto);
    this.#user.set(user);

    localStorage.setItem('user', JSON.stringify(userAuthDto));
  }

  updateFollowingStreamers(followingStreamers: string[]) {
    this.#followingStreamIds.set(followingStreamers);
  }

  updateBlockedStreamers(blockedStreamerIds: string[]) {
    this.#blockedStreamIds.set(blockedStreamerIds);
  }

  login(credentials: UserLoginDto) {
    return this.authProxyService.login(credentials).pipe(
      tap((userAuthDto) => this.setUser(userAuthDto)),
      switchMap(() => this.streamHub.disconnect()),
      switchMap(() => this.streamHub.buildAndConnect(this.user()?.accessToken)),
      tap(() => this.login$.next())
    );
  }

  register(credentials: UserRegisterDto) {
    return this.authProxyService.register(credentials).pipe(
      tap((userAuthDto) => this.setUser(userAuthDto)),
      switchMap(() => this.streamHub.disconnect()),
      switchMap(() => this.streamHub.buildAndConnect(this.user()?.accessToken)),
      tap(() => this.login$.next())
    );
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

    return this.authProxyService.refreshToken(userRefreshTokenDto).pipe(
      tap((userAuthDto) => this.setUser(userAuthDto)),
      switchMap(() => this.streamHub.disconnect()),
      switchMap(() =>
        this.streamHub.buildAndConnect(this.user()?.accessToken).pipe(
          map(() => this.user()!),
          tap(() => this.login$.next())
        )
      )
    );
  }

  logout() {
    this.#user.set(undefined);
    this.#followingStreamIds.set([]);
    this.#blockedStreamIds.set([]);
    localStorage.removeItem('user');
    this.streamHub
      .disconnect()
      .pipe(switchMap(() => this.streamHub.buildAndConnect()))
      .subscribe();
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

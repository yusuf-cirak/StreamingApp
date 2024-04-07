import { Injectable, computed, inject, signal } from '@angular/core';
import { CurrentUser } from '../../../models/current-user';
import { AuthProxyService } from './auth-proxy.service';
import { UserLoginDto } from '../../../dtos/user-login-dto';
import { UserRegisterDto } from '../../../dtos/user-register-dto';
import { UserRefreshTokenDto } from '../../../dtos/user-refresh-token-dto';
import { UserAuthDto } from '../dtos/user-auth-dto';
import { lastValueFrom, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly authProxyService = inject(AuthProxyService);

  #user = signal<CurrentUser | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  readonly userId = computed(() => this.user()?.id);

  readonly isAuthenticated = computed(() => {
    const user = this.user();

    return !!user && user.tokenExpiration > new Date(Date.now());
  });

  getUserFromLocalStorage(): UserAuthDto | null {
    const user = localStorage.getItem('user');
    return user?.length ? JSON.parse(user) : null;
  }

  getUserFromAuthDto(userAuthDto: UserAuthDto): CurrentUser {
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
    if (userAuthDto) {
      const tokenExpiration = new Date(userAuthDto.tokenExpiration);

      userAuthDto.tokenExpiration = tokenExpiration;

      this.setUser(userAuthDto);

      if (tokenExpiration < new Date(Date.now())) {
        await lastValueFrom(
          this.refreshToken(this.getUserFromAuthDto(userAuthDto))
        ).catch((err) => {});
      }
    }
  }

  setUser(userAuthDto: UserAuthDto) {
    const user = this.getUserFromAuthDto(userAuthDto);
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
}

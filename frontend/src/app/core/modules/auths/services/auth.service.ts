import { Injectable, computed, inject, signal } from '@angular/core';
import { CurrentUser } from '../../../models/current-user';
import { AuthProxyService } from './auth-proxy.service';
import { UserLoginDto } from '../../../dtos/user-login-dto';
import { UserRegisterDto } from '../../../dtos/user-register-dto';
import { UserRefreshTokenDto } from '../../../dtos/user-refresh-token-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly authProxyService = inject(AuthProxyService);

  #user = signal<CurrentUser | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  readonly userId = computed(() => this.user()?.id);

  readonly isAuthenticated = computed(() => !!this.user());

  getUserFromLocalStorage(): CurrentUser | null {
    const user = localStorage.getItem('user');
    return user?.length ? JSON.parse(user) : null;
  }

  setUser(user: CurrentUser) {
    this.#user.update(() => user);
    localStorage.setItem('user', JSON.stringify(user));
  }

  login(credentials: UserLoginDto) {
    return this.authProxyService.login(credentials);
  }

  register(credentials: UserRegisterDto) {
    return this.authProxyService.register(credentials);
  }

  refreshToken() {
    const user = this.user();
    const userRefreshTokenDto: UserRefreshTokenDto = {
      refreshToken: user?.refreshToken!,
      userId: user?.id!,
    };
    return this.authProxyService.refreshToken(userRefreshTokenDto);
  }

  logout() {
    this.#user.set(undefined);
    localStorage.removeItem('user');
  }
}

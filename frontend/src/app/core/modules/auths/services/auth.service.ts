import { Injectable, computed, inject, signal } from '@angular/core';
import { CurrentUser } from '../../../models/current-user';
import { AuthProxyService } from './auth-proxy.service';
import { UserLoginDto } from '../../../components/dtos/user-login-dto';
import { UserRegisterDto } from '../../../components/dtos/user-register-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  readonly authProxyService = inject(AuthProxyService);

  #user = signal<CurrentUser | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  readonly isAuthenticated = computed(() => !!this.user());

  getUserFromLocalStorage(): CurrentUser | null {
    const user = localStorage.getItem('user');
    return user?.length ? JSON.parse(user) : null;
  }

  setUser(user: CurrentUser) {
    this.#user.set(user);
    localStorage.setItem('user', JSON.stringify(user));
  }

  login(credentials: UserLoginDto) {
    return this.authProxyService.login(credentials);
  }

  register(credentials: UserRegisterDto) {
    return this.authProxyService.register(credentials);
  }

  refreshToken() {
    const token = this.user()?.refreshToken;
    return this.authProxyService.refreshToken(token!);
  }

  logout() {
    this.#user.set(undefined);
    localStorage.removeItem('user');
  }
}

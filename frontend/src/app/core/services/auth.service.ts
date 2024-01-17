import { Injectable, computed, signal } from '@angular/core';
import { User } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  #user = signal<User | undefined>(undefined);

  readonly user = this.#user.asReadonly();

  readonly isAuthenticated = computed(() => !!!this.user());
}

import { Component, effect, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';
import { AuthService, LayoutService } from './core/services';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent {
  readonly primeNgConfig = inject(PrimeNGConfig);
  readonly layoutService = inject(LayoutService);
  readonly authService = inject(AuthService);
  readonly router = inject(Router);

  constructor() {
    effect(() => {
      if (!this.authService.isAuthenticated()) {
        if (this.router.url.includes('creator')) {
          this.router.navigateByUrl('/');
        }
      }
    });
  }

  ngOnInit() {
    this.primeNgConfig.ripple = true;
    const user = this.authService.getUserFromLocalStorage();
    if (user) {
      const tokenExpiration = new Date(user.tokenExpiration);
      if (tokenExpiration > new Date(Date.now())) {
        this.authService.setUser(user);
      }
    }
  }
}

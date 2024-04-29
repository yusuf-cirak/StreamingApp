import { Component, OnDestroy, OnInit, effect, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { PrimeNGConfig } from 'primeng/api';
import { AuthService, LayoutService } from './core/services';
import { StreamHub } from './core/hubs/stream-hub';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
})
export class AppComponent {
  readonly layoutService = inject(LayoutService);
  readonly authService = inject(AuthService);
  readonly router = inject(Router);
  readonly streamHub = inject(StreamHub);

  constructor() {
    effect(() => {
      if (!this.authService.isAuthenticated()) {
        if (this.router.url.includes('creator')) {
          this.router.navigateByUrl('/');
        }
      }
    });
  }
}

import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { SearchComponent } from '../../../../modules/search/search-bar/search-bar.component';
import { ClapperBoardIcon } from '../../../../../shared/icons/clapper-board.icon';
import { RouterLink } from '@angular/router';
import { ProfilebarComponent } from '../profilebar/profilebar.component';
import { AuthService } from '../../../../modules/auths/services/auth.service';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { AuthComponent } from '../../../../modules/auths/auth.component';
@Component({
  selector: 'app-main-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  imports: [
    LogoComponent,
    SearchComponent,
    ClapperBoardIcon,
    RouterLink,
    ProfilebarComponent,
    RippleModule,
    ButtonModule,
    AuthComponent,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavbarComponent {
  readonly authService = inject(AuthService);

  #authModalVisible = signal(false);
  readonly authModalVisible = this.#authModalVisible.asReadonly();

  openAuthModal() {
    this.#authModalVisible.set(true);
  }

  closeAuthModal() {
    this.#authModalVisible.set(false);
  }
}

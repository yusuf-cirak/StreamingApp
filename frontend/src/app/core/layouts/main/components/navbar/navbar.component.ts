import { Component, inject } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { SearchComponent } from '../search/search.component';
import { ClapperBoardIcon } from '../../../../../shared/icons/clapper-board.icon';
import { RouterLink } from '@angular/router';
import { ProfilebarComponent } from '../profilebar/profilebar.component';
import { AuthService } from '../../../../services/auth.service';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
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
  ],
})
export class NavbarComponent {
  readonly authService = inject(AuthService);
}

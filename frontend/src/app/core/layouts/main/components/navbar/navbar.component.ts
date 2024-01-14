import { Component, inject } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { SearchComponent } from '../search/search.component';
import { HlmButtonDirective } from '../../../../../../../libs/spartan/ui-button-helm/src/lib/hlm-button.directive';
import { ClapperBoardIcon } from '../../../../../shared/icons/clapper-board.icon';
import { RouterLink } from '@angular/router';
import { ProfilebarComponent } from '../profilebar/profilebar.component';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-main-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  imports: [
    LogoComponent,
    SearchComponent,
    HlmButtonDirective,
    ClapperBoardIcon,
    RouterLink,
    ProfilebarComponent,
  ],
})
export class NavbarComponent {
  readonly authService = inject(AuthService);
}

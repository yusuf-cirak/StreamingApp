import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { SearchComponent } from '../search/search.component';
import { HlmButtonDirective } from '../../../../../../../libs/spartan/ui-button-helm/src/lib/hlm-button.directive';
import { ClapperBoardIcon } from '../../../../../shared/icons/clapper-board/clapper-board.icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  imports: [
    LogoComponent,
    SearchComponent,
    HlmButtonDirective,
    ClapperBoardIcon,
    RouterLink,
  ],
})
export class NavbarComponent {}

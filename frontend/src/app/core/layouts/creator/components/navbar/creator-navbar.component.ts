import { Component, inject } from '@angular/core';
import { CreatorLogoComponent } from '../logo/logo.component';
import { RouterLink } from '@angular/router';
import { ProfilebarComponent } from '../../../main/components/profilebar/profilebar.component';
import { ArrowRightIcon } from '../../../../../shared/icons/arrow-right-icon';
import { NavbarDashobardsComponent } from './navbar-dashobards/navbar-dashobards.component';

@Component({
  selector: 'app-creator-navbar',
  standalone: true,
  templateUrl: './creator-navbar.component.html',
  imports: [
    CreatorLogoComponent,
    ArrowRightIcon,
    RouterLink,
    ProfilebarComponent,
    NavbarDashobardsComponent,
  ],
})
export class CreatorNavbarComponent {}

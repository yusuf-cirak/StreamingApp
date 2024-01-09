import { Component } from '@angular/core';
import { LogoComponent } from '../logo/logo.component';
import { SearchComponent } from '../search/search.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  imports: [LogoComponent, SearchComponent],
})
export class NavbarComponent {}

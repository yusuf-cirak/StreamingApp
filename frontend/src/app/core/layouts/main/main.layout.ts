import { Component } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [NavbarComponent],
  templateUrl: './main.layout.html',
})
export class MainLayout {}

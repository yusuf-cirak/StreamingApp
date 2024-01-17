import { RouterOutlet } from '@angular/router';
import { Component, inject } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NgIf, NgStyle } from '@angular/common';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [
    NavbarComponent,
    SidebarComponent,
    NgStyle,
    NgIf,
    RouterOutlet,
    TooltipModule,
  ],
  templateUrl: './main.layout.html',
})
export class MainLayout {}

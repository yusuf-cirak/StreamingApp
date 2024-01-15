import { Component } from '@angular/core';
import { NgIf, NgStyle } from '@angular/common';
import { CreatorNavbarComponent } from './components/navbar/creator-navbar.component';
import { CreatorSidebarComponent } from './components/sidebar/creator-sidebar.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-creator',
  standalone: true,
  imports: [
    CreatorNavbarComponent,
    CreatorSidebarComponent,
    NgStyle,
    NgIf,
    RouterOutlet,
  ],
  templateUrl: './creator.layout.html',
})
export class CreatorLayout {}

import { Component, inject } from '@angular/core';
import { NgIf, NgStyle } from '@angular/common';
import { LayoutService } from '../../services/layout.service';
import { CreatorNavbarComponent } from './components/navbar/creator-navbar.component';
import { CreatorSidebarComponent } from './components/sidebar/creator-sidebar.component';

@Component({
  selector: 'app-creator',
  standalone: true,
  imports: [CreatorNavbarComponent, CreatorSidebarComponent, NgStyle, NgIf],
  templateUrl: './creator.layout.html',
})
export class CreatorLayout {
  readonly layoutService = inject(LayoutService);
}

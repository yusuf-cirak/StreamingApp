import { Component, Input, input } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { StreamComponent } from '../../modules/streams/stream.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [NavbarComponent, SidebarComponent, StreamComponent],
  templateUrl: './main.layout.html',
})
export class MainLayout {
  streamerName = input<string>();
}

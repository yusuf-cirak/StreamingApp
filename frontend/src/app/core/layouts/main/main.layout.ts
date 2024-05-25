import { Component, Input, input } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { StreamComponent } from '../../modules/streams/stream.component';
import { StreamersShowCaseComponent } from './components/streamers-show-case/streamers-show-case.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [
    NavbarComponent,
    SidebarComponent,
    StreamComponent,
    StreamersShowCaseComponent,
  ],
  templateUrl: './main.layout.html',
})
export class MainLayout {
  streamerName = input<string>();
}

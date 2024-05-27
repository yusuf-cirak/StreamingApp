import { Component, Input, input } from '@angular/core';
import { StreamComponent } from '../../modules/streams/stream.component';
import { NavbarComponent } from '../main/components/navbar/navbar.component';
import { SidebarComponent } from '../main/components/sidebar/sidebar.component';
import { StreamersShowCaseComponent } from '../main/components/streamers-show-case/streamers-show-case.component';

@Component({
  selector: 'app-stream-layout',
  standalone: true,
  imports: [
    NavbarComponent,
    SidebarComponent,
    StreamComponent,
    StreamersShowCaseComponent,
  ],
  templateUrl: './stream.layout.html',
})
export class StreamLayout {
  streamerName = input<string>();
}

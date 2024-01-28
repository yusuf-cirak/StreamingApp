import { Component, Input } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { LiveStreamComponent } from './components/stream/components/live-stream/live-stream.component';
import { OfflineStreamComponent } from './components/stream/components/offline-stream/offline-stream.component';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [
    NavbarComponent,
    SidebarComponent,
    LiveStreamComponent,
    OfflineStreamComponent,
  ],
  templateUrl: './main.layout.html',
})
export class MainLayout {
  @Input('streamer') streamer?: string;
}

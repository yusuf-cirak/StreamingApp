import { RouterOutlet } from '@angular/router';
import { Component, Input, inject } from '@angular/core';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { NgIf, NgStyle } from '@angular/common';
import { TooltipModule } from 'primeng/tooltip';
import { StreamComponent } from './components/stream/stream.component';

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
    StreamComponent,
  ],
  templateUrl: './main.layout.html',
})
export class MainLayout {
  @Input('streamer') streamer?: string;

  ngOnInit() {
    console.log(this.streamer);
  }
}

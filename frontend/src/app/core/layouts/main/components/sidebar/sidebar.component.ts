import { Streamer } from './../../../../modules/streamers/models/streamer';
import { NgClass } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  ViewChild,
  effect,
  inject,
  signal,
} from '@angular/core';
import { CollapseLeftIcon } from '../../../../../shared/icons/collapse-left';
import { ExpandRightIcon } from '../../../../../shared/icons/expand-right';
import { expandCollapseAnimation } from '../../../../../shared/animations/expand-collapse-animation';
import { LayoutService } from '@streaming-app/core';
import { StreamersComponent } from '../../../../modules/streamers/components/streamers.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgClass, CollapseLeftIcon, ExpandRightIcon, StreamersComponent],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  animations: [expandCollapseAnimation],
})
export class SidebarComponent {
  readonly layoutService = inject(LayoutService);

  readonly #sidebarOpen = signal(true);
  readonly sidebarOpen = this.#sidebarOpen.asReadonly();

  @ViewChild('sidebarRef') sidebarRef: ElementRef | undefined;

  constructor() {
    effect(() => {
      const sidebar = this.sidebarRef?.nativeElement;

      if (this.sidebarOpen()) {
        sidebar?.classList.remove('sidebar-closed');
      } else {
        sidebar?.classList.add('sidebar-closed');
      }
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.#sidebarOpen.set(event.target.innerWidth > 640);
  }

  toggleSidebar() {
    const value = this.sidebarOpen();
    this.#sidebarOpen.set(!value);
  }
}

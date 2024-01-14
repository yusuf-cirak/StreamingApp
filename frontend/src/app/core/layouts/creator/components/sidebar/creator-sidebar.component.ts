import { NgClass } from '@angular/common';
import {
  Component,
  ElementRef,
  HostListener,
  ViewChild,
  effect,
  signal,
} from '@angular/core';
import { CollapseLeftIcon } from '../../../../../shared/icons/collapse-left';
import { ExpandRightIcon } from '../../../../../shared/icons/expand-right';
import { expandCollapseAnimation } from '../../../../../shared/animations/expand-collapse-animation';
import { StreamIconComponent } from '../../../../../shared/icons/stream-icon';
import { ChatIconComponent } from '../../../../../shared/icons/chat-icon';
import { KeyIconComponent } from '../../../../../shared/icons/key-icon';
import { CommunityIconComponent } from '../../../../../shared/icons/community-icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-creator-sidebar',
  standalone: true,
  imports: [
    NgClass,
    RouterLink,
    CollapseLeftIcon,
    ExpandRightIcon,
    StreamIconComponent,
    ChatIconComponent,
    KeyIconComponent,
    CommunityIconComponent,
  ],
  templateUrl: './creator-sidebar.component.html',
  animations: [expandCollapseAnimation],
})
export class CreatorSidebarComponent {
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

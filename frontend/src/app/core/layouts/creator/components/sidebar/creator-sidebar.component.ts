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
import { StreamIconComponent } from '../../../../../shared/icons/stream-icon';
import { ChatIconComponent } from '../../../../../shared/icons/chat-icon';
import { KeyIconComponent } from '../../../../../shared/icons/key-icon';
import { CommunityIconComponent } from '../../../../../shared/icons/community-icon';
import { RouterLink } from '@angular/router';
import { LayoutService } from '../../../../services/layout.service';

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
  readonly layoutService = inject(LayoutService);
}

import { NgClass } from '@angular/common';
import {
  Component,

  inject,
} from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { LayoutService } from '../../../../services/layout.service';
import { AuthService } from '@streaming-app/core';
import {
  ChatIconComponent,
  CollapseLeftIcon,
  CommunityIconComponent,
  ExpandRightIcon,
  KeyIconComponent,
  StreamIconComponent,
} from '@streaming-app/shared/icons';
import { expandCollapseAnimation } from '../../../../../shared/animations/expand-collapse-animation';

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
  readonly currentUser = inject(AuthService).user;

  readonly router = inject(Router);

  navigateToStream() {
    this.router.navigate(['/creator/stream'], {
      state: { streamerName: this.currentUser()?.username },
    });
  }
}

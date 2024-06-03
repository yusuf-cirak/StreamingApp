import { NgClass } from '@angular/common';
import { Component, computed, DebugNode, inject, signal } from '@angular/core';
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
import { CtrlDirective } from '../../../../../shared/directives/ctrl.directive';
import { CurrentCreatorService } from '../../services/current-creator-service';
import { AuthorizationDirective } from '../../../../../shared/directives/authorization.directive';
import { HintComponent } from '../../../../components/hint/hint.component';
import { CreatorAuthService } from '../../services/creator-auth-service';
import { UserAuthorizationService } from '../../../../services/user-authorization.service';

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
    CtrlDirective,
    AuthorizationDirective,
    HintComponent,
  ],
  templateUrl: './creator-sidebar.component.html',
  animations: [expandCollapseAnimation],
})
export class CreatorSidebarComponent {
  readonly layoutService = inject(LayoutService);
  readonly creatorService = inject(CurrentCreatorService);
  readonly creatorAuthService = inject(CreatorAuthService);
  readonly authService = inject(AuthService);
  readonly userAuthorizationService = inject(UserAuthorizationService);

  readonly canNavigateToKeyPage = computed(() => {
    const { roles } = this.creatorAuthService.keyPageRequirements();

    return this.userAuthorizationService.checkRoles(roles);
  });

  readonly canNavigateToChatSettingsPage = computed(() => {
    const { roles, operationClaims } =
      this.creatorAuthService.chatSettingsPageRequirements();

    return (
      this.userAuthorizationService.checkRoles(roles) ||
      this.userAuthorizationService.checkOperationClaims(operationClaims)
    );
  });

  readonly canNavigateToCommunitySettingsPage = computed(() => {
    const { roles, operationClaims } =
      this.creatorAuthService.communityPageRequirements();

    return (
      this.userAuthorizationService.checkRoles(roles) ||
      this.userAuthorizationService.checkOperationClaims(operationClaims)
    );
  });

  readonly router = inject(Router);

  readonly ctrlPressed = signal(false);

  navigateToStream() {
    const url = `/creator/${this.creatorService.streamerName()}/stream`;
    if (this.ctrlPressed()) {
      window.open(url, '_blank')?.focus();
    } else {
      this.router.navigate([url], {
        state: { streamerName: this.creatorService.streamerName() },
      });
    }
  }

  navigateTo(url: string) {
    url = `/creator/${this.creatorService.streamerName()}/${url}`;
    if (this.ctrlPressed()) {
      window.open(url, '_blank')?.focus();
    } else {
      this.router.navigate([url]);
    }
  }

  navigateInNewTab(url: string) {
    return () => window.open(url, '_blank')?.focus();
  }
}

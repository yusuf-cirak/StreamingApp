import { Component, computed, inject, input, output } from '@angular/core';
import { AuthService, User } from '@streaming-app/core';
import {
  AuthorizationDirective,
  TextHashColorDirective,
} from '@streaming-app/shared/directives';
import { StreamFacade } from '../../../../streams/services/stream.facade';
import { BlockIcon } from '../../../../../../shared/icons/block.icon';
import { TooltipModule } from 'primeng/tooltip';
import { StreamBlockUserDto } from '../../../models/stream-block-user-dto';
import { UserRoleDto } from '../../../../auths/models/role';
import { UserOperationClaimDto } from '../../../../auths/models/operation-claim';

@Component({
  selector: 'app-community-viewer',
  standalone: true,
  imports: [
    TextHashColorDirective,
    BlockIcon,
    TooltipModule,
    AuthorizationDirective,
  ],
  templateUrl: './community-viewer.component.html',
})
export class CommunityViewerComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly authService = inject(AuthService);

  viewer = input.required<User>();

  readonly streamerId = computed(
    () => this.streamFacade.liveStream().user.id ?? ''
  );

  roles: UserRoleDto[] = [
    { name: 'Admin' },
    { name: 'SuperModerator', value: this.streamerId() },
  ];

  operationClaims: UserOperationClaimDto[] = [
    { name: 'BlockUser', value: this.streamerId() },
  ];

  isSelf = computed(
    () => this.viewer().username === this.authService.user()?.username
  );

  blockViewer = output<StreamBlockUserDto>();

  block() {
    const streamerId = this.streamFacade.liveStream().user.id ?? ('' as string);
    const blockedUserId = this.viewer().id;
    this.blockViewer.emit({ streamerId, blockedUserId });
  }
}

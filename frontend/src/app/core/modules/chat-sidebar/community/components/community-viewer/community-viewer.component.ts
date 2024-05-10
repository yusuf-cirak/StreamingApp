import { Component, computed, inject, input, output } from '@angular/core';
import { AuthService, User } from '@streaming-app/core';
import { TextHashColorDirective } from '@streaming-app/shared/directives';
import { StreamFacade } from '../../../../streams/services/stream.facade';
import { BlockIcon } from '../../../../../../shared/icons/block.icon';
import { TooltipModule } from 'primeng/tooltip';
import { StreamBlockUserDto } from '../../../models/stream-block-user-dto';

@Component({
  selector: 'app-community-viewer',
  standalone: true,
  imports: [TextHashColorDirective, BlockIcon, TooltipModule],
  templateUrl: './community-viewer.component.html',
})
export class CommunityViewerComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly authService = inject(AuthService);

  viewer = input.required<User>();

  // TODO: Only authorized users can block other users
  // isHost = computed(
  //   () => this.authService.user()?.username === this.streamFacade.streamerName()
  // );

  isSelf = computed(
    () => this.viewer().username === this.authService.user()?.username && false
  );

  blockViewer = output<StreamBlockUserDto>();

  block() {
    const streamerId = this.streamFacade.liveStream().user.id ?? ('' as string);
    const blockedUserId = this.viewer().id;
    this.blockViewer.emit({ streamerId, blockedUserId });
  }
}

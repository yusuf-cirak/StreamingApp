import { Injectable, computed, inject } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { StreamDto } from '../../../streams/contracts/stream-dto';
import { StreamFacade } from '../../../streams/services/stream.facade';

@Injectable({
  providedIn: 'root',
})
export class ChatAuthService {
  readonly authService = inject(AuthService);

  readonly streamFacade = inject(StreamFacade);

  readonly chatDelayMessage = computed(() => {
    const second = this.streamFacade.liveStream()?.streamOption.chatDelaySecond;
    return second ? `Slow mode with ${second} second delay` : null;
  });

  chatErrorMessage = computed(() =>
    this.canUserSendMessage(this.streamFacade.liveStream()!)
  );

  canUserSendMessage(liveStream: StreamDto): string | null {
    const option = liveStream.streamOption;
    if (option.chatDisabled) {
      return 'Chat is disabled';
    }

    const authenticated = this.authService.isAuthenticated();
    if (!authenticated) {
      return 'You must be logged in to use chat';
    }

    if (option.mustBeFollower) {
      const followingStreamers = this.authService.followingStreamers();
      const isFollowing = followingStreamers?.includes(liveStream.user.id);

      if (!isFollowing) {
        return 'Followers only';
      }
    }

    return null;
  }
}

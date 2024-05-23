import { Injectable, computed, inject } from '@angular/core';
import { AuthService } from '@streaming-app/core';
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

  chatErrorMessage = computed(() => {
    const isAuth = this.authService.isAuthenticated();

    if (!isAuth) {
      return 'You must be logged in to use chat';
    }

    const liveStream = this.streamFacade.liveStream();
    const option = liveStream.streamOption;

    if (option.chatDisabled) {
      return 'Chat is disabled';
    }

    const blockedStreamerIds = this.authService.blockedStreamIds();

    if (blockedStreamerIds.includes(liveStream.user.id)) {
      return 'You are blocked from this stream chat';
    }

    if (option.mustBeFollower) {
      const followingStreamers = this.authService.followingStreamIds();
      const isFollowing = followingStreamers?.includes(liveStream.user.id);

      if (!isFollowing) {
        return 'Followers only';
      }
    }

    return null;
  });
}

import { Injectable, computed, inject } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from '../../streams/services/stream.facade';

@Injectable({
  providedIn: 'root',
})
export class ChatAuthService {
  readonly authService = inject(AuthService);

  readonly streamFacade = inject(StreamFacade);

  readonly chatDelayMessage = computed(() => {
    const second = this.streamFacade.liveStream()?.options.chatDelaySecond;
    return second ? `Slow mode with ${second} second delay` : null;
  });

  chatErrorMessage = computed(() =>
    this.canUserSendMessage(this.streamFacade.liveStream()!)
  );

  canUserSendMessage(liveStream: LiveStreamDto): string | null {
    const options = liveStream.options;
    if (options.chatDisabled) {
      return 'Chat is disabled';
    }

    const authenticated = this.authService.isAuthenticated();
    if (!authenticated) {
      return 'You must be logged in to use chat';
    }

    if (options.mustBeFollower) {
      const user = this.authService.user();
      const isFollowing = user?.followingStreamers?.includes(
        liveStream.user.id
      );

      if (!isFollowing) {
        return 'Followers only';
      }
    }

    return null;
  }
}

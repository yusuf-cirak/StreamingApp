import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { tap } from 'rxjs';
import { AuthService } from '@streaming-app/core';
import { StreamProxyService } from '../../streams/services/stream-proxy.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  readonly httpClientService = inject(HttpClientService);
  readonly streamProxyService = inject(StreamProxyService);

  readonly authService = inject(AuthService);

  getFollowingStreamers() {
    return this.streamProxyService.getFollowing().pipe(
      tap((followingStreamers) => {
        this.authService.updateFollowingStreamers(
          followingStreamers.map((fs) => fs.user.id)
        );
      })
    );
  }

  getBlockedStreamers() {
    return this.streamProxyService.getBlocked().pipe(
      tap((blockedStreamers) => {
        this.authService.updateBlockedStreamers(
          blockedStreamers.map((fs) => fs.user.id)
        );
      })
    );
  }
}

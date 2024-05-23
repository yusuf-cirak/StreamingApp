import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { UserProxyService } from './user-proxy.service';
import { tap } from 'rxjs';
import { AuthService } from '@streaming-app/core';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  readonly httpClientService = inject(HttpClientService);
  readonly userProxyService = inject(UserProxyService);

  readonly authService = inject(AuthService);

  getFollowingStreamers() {
    return this.userProxyService.getFollowingStreamers().pipe(
      tap((followingStreamers) => {
        this.authService.updateFollowingStreamers(
          followingStreamers.map((fs) => fs.user.id)
        );
      })
    );
  }

  getBlockedStreamers() {
    return this.userProxyService.getBlockedStreamers().pipe(
      tap((blockedStreamers) => {
        this.authService.updateBlockedStreamers(
          blockedStreamers.map((fs) => fs.user.id)
        );
      })
    );
  }
}

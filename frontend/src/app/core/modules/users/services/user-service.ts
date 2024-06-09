import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { tap } from 'rxjs';
import { AuthService } from '@streaming-app/core';
import { StreamProxyService } from '../../streams/services/stream-proxy.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly httpClientService = inject(HttpClientService);
  private readonly streamProxyService = inject(StreamProxyService);

  private readonly authService = inject(AuthService);

  uploadProfileImage(profileImageDto: UpdateProfileImageDto) {
    const formData = Object.entries(profileImageDto).reduce(
      (acc, [key, value]) => {
        acc.append(key, value);

        return acc;
      },
      new FormData()
    );
    return this.httpClientService.post<string>(
      {
        controller: 'users',
        action: 'profile-image',
      },
      formData
    );
  }

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

export type UpdateProfileImageDto = {
  profileImageUrl: string;
  profileImage: File;
};

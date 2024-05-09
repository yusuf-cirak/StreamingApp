import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../../shared/services/http-client.service';
import { User } from '../../../../models';
import { StreamBlockUserDto } from '../../models/stream-block-user-dto';

@Injectable()
export class CommunityProxyService {
  private readonly httpClient = inject(HttpClientService);

  getViewers(streamerName: string) {
    return this.httpClient.get<User[]>({
      controller: 'streams',
      action: 'viewers',
      routeParams: [streamerName],
    });
  }

  blockUser(blockUserDto: StreamBlockUserDto) {
    return this.httpClient.post(
      {
        controller: 'stream-blocked-users',
      },
      blockUserDto
    );
  }

  unblockUser(unblockUserDto: StreamBlockUserDto) {
    return this.httpClient.put(
      {
        controller: 'stream-blocked-users',
      },
      unblockUserDto
    );
  }
}

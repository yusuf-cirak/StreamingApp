import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { GetStreamBlockedUserDto } from '../contracts/get-stream-blocked-user-dto';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CommunitySettingsService {
  private readonly httpClientService = inject(HttpClientService);

  getBlockedUsers(streamerId: string): Observable<GetStreamBlockedUserDto[]> {
    return this.httpClientService.get<GetStreamBlockedUserDto[]>({
      controller: 'stream-blocked-users',
      action: 'all',
      routeParams: [streamerId],
    });
  }

  unblockUsers(streamerId: string, userIds: string[]): Observable<boolean> {
    return this.httpClientService.put<boolean>(
      {
        controller: 'stream-blocked-users',
      },
      { streamerId, blockedUserIds: userIds }
    );
  }
}

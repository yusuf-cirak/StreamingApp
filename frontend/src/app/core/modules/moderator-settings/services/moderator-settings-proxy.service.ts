import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { Observable } from 'rxjs';
import { GetStreamModeratorDto } from '../contracts/get-stream-moderator-dto';
import { UpsertStreamModeratorDto } from '../contracts/upsert-stream-moderator-dto';

@Injectable({ providedIn: 'root' })
export class ModeratorSettingsProxyService {
  private readonly httpClientService = inject(HttpClientService);

  getModerators(): Observable<GetStreamModeratorDto[]> {
    return this.httpClientService.get<GetStreamModeratorDto[]>({
      controller: 'streams',
      action: 'moderators',
    });
  }

  upsertModerators(upsertModeratorsDto: UpsertStreamModeratorDto) {
    return this.httpClientService.post<boolean>(
      {
        controller: 'users',
        action: 'stream-moderators',
      },
      upsertModeratorsDto
    );
  }

  removeModerators(userIds: string[]) {
    return this.httpClientService.delete<boolean>(
      {
        controller: 'users',
        action: 'stream-moderators',
      },
      { userIds: userIds }
    );
  }
}

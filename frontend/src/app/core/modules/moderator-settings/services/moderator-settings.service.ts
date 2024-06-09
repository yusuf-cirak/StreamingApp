import { inject, Injectable } from '@angular/core';
import { ModeratorSettingsProxyService } from './moderator-settings-proxy.service';
import { UpsertStreamModeratorDto } from '../contracts/upsert-stream-moderator-dto';
import { Subject } from 'rxjs';

@Injectable()
export class ModeratorSettingsService {
  private readonly moderatorSettingsProxyService = inject(
    ModeratorSettingsProxyService
  );

  readonly update$ = new Subject<void>();

  getModerators() {
    return this.moderatorSettingsProxyService.getModerators();
  }

  upsertModerators(upsertModeratorsDto: UpsertStreamModeratorDto) {
    return this.moderatorSettingsProxyService.upsertModerators(
      upsertModeratorsDto
    );
  }

  removeModerators(userIds: string[]) {
    return this.moderatorSettingsProxyService.removeModerators(userIds);
  }
}

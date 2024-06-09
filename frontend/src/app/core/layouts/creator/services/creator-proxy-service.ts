import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { User } from '../../../models';

@Injectable({ providedIn: 'root' })
export class CreatorProxyService {
  private readonly httpClientService = inject(HttpClientService);

  getModeratingStreamers() {
    return this.httpClientService.get<User[]>({
      controller: 'streams',
      action: 'moderating',
    });
  }
}

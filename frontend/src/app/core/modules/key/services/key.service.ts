import { Injectable, inject } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class KeyService {
  private readonly httpClientService = inject(HttpClientService);
  private readonly authService = inject(AuthService);

  get() {
    return this.httpClientService.get<string>({
      controller: 'stream-options',
      action: 'key',
      routeParams: [this.authService.user()?.id!], // todo: get this from parameter, moderators will try to access this
    });
  }

  generate(): Observable<string> {
    const streamerId = this.authService.userId() as string;
    return this.httpClientService.post<string>(
      {
        controller: 'stream-options',
        action: 'key',
      },
      { streamerId }
    );
  }
}

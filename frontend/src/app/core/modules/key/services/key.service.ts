import { Injectable, inject } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { HttpClientService } from '@streaming-app/shared/services';

@Injectable({
  providedIn: 'root',
})
export class KeyService {
  private readonly httpClientService = inject(HttpClientService);
  private readonly authService = inject(AuthService);

  get() {
    return this.httpClientService.get<string>(
      {
        controller: 'stream-options',
        action: 'key',
        responseType: 'text',
      },
      this.authService.user()?.id
    );
  }
}

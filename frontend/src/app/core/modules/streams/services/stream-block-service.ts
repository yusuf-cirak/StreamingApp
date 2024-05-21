import { inject, Injectable } from '@angular/core';
import { AuthService } from '@streaming-app/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { StreamBlockUserActionDto } from '../../../hubs/dtos/stream-block-user-action-dto';

@Injectable({ providedIn: 'root' })
export class StreamBlockService {
  private readonly httpClient = inject(HttpClientService);
  private readonly authService = inject(AuthService);
}

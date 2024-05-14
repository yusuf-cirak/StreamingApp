import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '@streaming-app/shared/services';
import { StreamDto } from '../../streams/contracts/stream-dto';

@Injectable({
  providedIn: 'root',
})
export class StreamerProxyService {
  readonly httpClientService = inject(HttpClientService);

  getLiveStreamers(): Observable<StreamDto[]> {
    return this.httpClientService.get<StreamDto[]>({
      controller: 'streams',
      action: 'live',
    });
  }
}

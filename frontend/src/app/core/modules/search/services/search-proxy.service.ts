import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { StreamDto } from '../../streams/contracts/stream-dto';

@Injectable({
  providedIn: 'root',
})
export class SearchProxyService {
  private readonly httpClientService = inject(HttpClientService);
  get(term: string) {
    return this.httpClientService.get<StreamDto[]>({
      controller: 'streams',
      action: 'search',
      queryStrings: [{ queryName: 'term', query: term }],
    });
  }
}

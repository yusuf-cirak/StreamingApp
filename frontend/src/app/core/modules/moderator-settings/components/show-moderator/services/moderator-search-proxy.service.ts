import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../../../shared/services/http-client.service';
import { User } from '../../../../../models';
import { map } from 'rxjs';
import { AuthService } from '@streaming-app/core';

@Injectable({
  providedIn: 'root',
})
export class ModeratorSearchProxyService {
  private readonly httpClient = inject(HttpClientService);
  private readonly authService = inject(AuthService);

  get(term: string) {
    return this.httpClient
      .get<User[]>({
        controller: 'users',
        action: 'search',
        queryStrings: [{ query: term, queryName: 'term' }],
      })
      .pipe(
        map((users) =>
          users.filter((user) => user.id !== this.authService.userId())
        )
      );
  }
}

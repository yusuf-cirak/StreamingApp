import { inject, Injectable } from '@angular/core';
import { CreatorProxyService } from './creator-proxy-service';
import { toSignal } from '@angular/core/rxjs-interop';
import { map, tap } from 'rxjs';
import { AuthService } from '@streaming-app/core';

@Injectable()
export class CreatorService {
  private readonly authService = inject(AuthService);
  private readonly creatorProxyService = inject(CreatorProxyService);

  readonly moderatingStreams = toSignal(
    this.creatorProxyService
      .getModeratingStreamers()
      .pipe(map((res) => [this.authService.user()!, ...res]))
  );
}

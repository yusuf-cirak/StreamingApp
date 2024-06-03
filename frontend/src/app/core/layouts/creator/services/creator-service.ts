import { inject, Injectable } from '@angular/core';
import { CreatorProxyService } from './creator-proxy-service';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable()
export class CreatorService {
  private readonly creatorProxyService = inject(CreatorProxyService);

  readonly moderatingStreams = toSignal(
    this.creatorProxyService.getModeratingStreamers()
  );
}

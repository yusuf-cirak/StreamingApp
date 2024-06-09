import { CanActivateFn, Router } from '@angular/router';
import { CreatorAuthService } from '../services/creator-auth-service';
import { inject } from '@angular/core';
import { UserAuthorizationService } from '../../../services/user-authorization.service';
import { map, tap } from 'rxjs';
import { StreamService } from '../../../modules/streams/services/stream.service';
import { AuthService } from '../../../services';
import { CurrentCreatorService } from '../services/current-creator-service';
import { StreamFacade } from '../../../modules/streams/services/stream.facade';

export const creatorLayoutGuard: CanActivateFn = (route, state) => {
  let streamerName = route.params['streamerName'];

  if (!streamerName) {
    streamerName =
      inject(AuthService).user()?.username ||
      inject(Router).getCurrentNavigation()?.extras.state!['streamerName']; // User state passed from the previous route
  }

  const currentCreatorService = inject(CurrentCreatorService);

  const streamService = inject(StreamService);

  const userAuthorizationService = inject(UserAuthorizationService);
  const creatorAuthService = inject(CreatorAuthService);
  const streamFacade = inject(StreamFacade);

  const router = inject(Router);

  return streamService.getStreamInfo(streamerName).pipe(
    tap((streamDto) => {
      currentCreatorService.update(streamDto.stream?.user!);
      streamFacade.setStream(streamDto);
    }),
    map(() => {
      return userAuthorizationService.check(
        creatorAuthService.pageRequirement.get('creator')
      )
        ? true
        : router.createUrlTree(['/']);
    })
  );
};

export const creatorPageGuard = (page: CreatorPage): CanActivateFn => {
  return (route, state) => {
    const userAuthorizationService = inject(UserAuthorizationService);
    const creatorAuthService = inject(CreatorAuthService);

    return userAuthorizationService.check(
      creatorAuthService.pageRequirement.get(page)
    )
      ? true
      : inject(Router).createUrlTree(['/']);
  };
};

export type CreatorPage =
  | 'key'
  | 'chat-settings'
  | 'community'
  | 'moderators'
  | 'stream'
  | 'creator';

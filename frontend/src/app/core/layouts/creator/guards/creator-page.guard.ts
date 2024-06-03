import { CanActivateFn, Router } from '@angular/router';
import { CreatorAuthService } from '../services/creator-auth-service';
import { inject } from '@angular/core';
import { UserAuthorizationService } from '../../../services/user-authorization.service';
import { map, tap } from 'rxjs';
import { StreamService } from '../../../modules/streams/services/stream.service';
import { AuthService } from '../../../services';
import { CurrentCreatorService } from '../services/current-creator-service';
import { StreamFacade } from '../../../modules/streams/services/stream.facade';

export const creatorGuard: CanActivateFn = (route, state) => {
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
      const { roles } = creatorAuthService.creatorPageRequirements();

      const rolesMatched = userAuthorizationService.checkRoles(roles);

      return rolesMatched ? true : router.createUrlTree(['/']);
    })
  );
};

export const chatSettingsGuard: CanActivateFn = (route, state) => {
  const userAuthorizationService = inject(UserAuthorizationService);
  const creatorAuthService = inject(CreatorAuthService);

  const { roles, operationClaims } =
    creatorAuthService.chatSettingsPageRequirements();

  const anyRoleMatched = userAuthorizationService.checkRoles(roles);
  const anyOperationClaimMatched =
    userAuthorizationService.checkOperationClaims(operationClaims);

  return anyRoleMatched || anyOperationClaimMatched;
};

export const keySettingsGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const userAuthorizationService = inject(UserAuthorizationService);
  const creatorAuthService = inject(CreatorAuthService);

  const { roles } = creatorAuthService.keyPageRequirements();

  return userAuthorizationService.checkRoles(roles)
    ? true
    : router.createUrlTree(['/']);
};

export const communitySettingsGuard: CanActivateFn = (route, state) => {
  const userAuthorizationService = inject(UserAuthorizationService);
  const creatorAuthService = inject(CreatorAuthService);

  const { roles, operationClaims } =
    creatorAuthService.communityPageRequirements();

  const anyRoleMatched = userAuthorizationService.checkRoles(roles);
  const anyOperationClaimMatched =
    userAuthorizationService.checkOperationClaims(operationClaims);

  return anyRoleMatched || anyOperationClaimMatched;
};

// todo: refactor this

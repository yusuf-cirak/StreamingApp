import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../../services';
import { UserRoleDto } from '../../../modules/auths/models/role';
import { Roles } from '../../../constants/roles';

export const streamerGuard: CanActivateFn = (route) => {
  let streamerId =
    route.paramMap.get('streamerId') ??
    inject(Router).getCurrentNavigation()?.extras.state!['streamerId'];

  if (!streamerId) {
    return false;
  }

  const authService = inject(AuthService);

  return (authService.user()?.roles || []).includes(
    (role: UserRoleDto) =>
      role.value === streamerId && Object.values(Roles).includes(role.name)
  );
};

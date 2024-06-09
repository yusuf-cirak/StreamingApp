import { inject, Injectable } from '@angular/core';
import { AuthService } from '..';
import { UserOperationClaimDto } from '../modules/auths/models/operation-claim';

@Injectable({
  providedIn: 'root',
})
export class UserAuthorizationService {
  private readonly authService = inject(AuthService);

  check(options: AuthorizationOptions) {
    const { roles = [], operationClaims = [], flags = [] } = options;

    const rolesMatched = this.checkRoles(roles);
    const operationClaimsMatched = this.checkOperationClaims(operationClaims);
    const flagsMatched = this.checkFlags(flags);

    return rolesMatched || operationClaimsMatched || flagsMatched;
  }

  checkOperationClaims(
    operationClaims: UserOperationClaimDto[],
    matchMode: 'any' | 'all' = 'any'
  ) {
    const userOperationClaims = this.authService.user()?.operationClaims || [];

    const hasRequiredOperationClaims = operationClaims.some((optClaim) => {
      return matchMode === 'any'
        ? userOperationClaims.some((userOperationClaim) => {
            return (
              userOperationClaim.name === optClaim.name &&
              userOperationClaim.value === optClaim.value
            );
          })
        : userOperationClaims.every((userOperationClaim) => {
            return (
              userOperationClaim.name === optClaim.name &&
              userOperationClaim.value === optClaim.value
            );
          });
    });

    return hasRequiredOperationClaims;
  }

  checkRoles(roles: UserOperationClaimDto[], matchMode: 'any' | 'all' = 'any') {
    const userRoles = this.authService.user()?.roles || [];

    return matchMode === 'any'
      ? roles.some((requiredRole) => {
          return userRoles.some((userRole) => {
            return (
              userRole.name === requiredRole.name &&
              (!requiredRole.value || userRole.value === requiredRole.value)
            );
          });
        })
      : roles.every((requiredRole) => {
          return userRoles.some((userRole) => {
            return (
              userRole.name === requiredRole.name &&
              (!requiredRole.value || userRole.value === requiredRole.value)
            );
          });
        });
  }

  checkFlags(flags: boolean[], matchMode: 'any' | 'all' = 'any') {
    return matchMode === 'any'
      ? flags.some((flag) => flag)
      : flags.every((flag) => flag);
  }
}

export type AuthorizationOptions = {
  roles?: UserOperationClaimDto[];
  operationClaims?: UserOperationClaimDto[];
  flags?: boolean[];
};

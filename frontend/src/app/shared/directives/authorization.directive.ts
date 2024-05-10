import {
  Directive,
  effect,
  inject,
  input,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AuthService } from '../../core/services';
import { UserOperationClaimDto } from '../../core/modules/auths/models/operation-claim';
import { UserRoleDto } from '../../core/modules/auths/models/role';

@Directive({
  selector: '[appAuthorization]',
  standalone: true,
})
export class AuthorizationDirective {
  authorizationParams = input<AuthorizationParams>(
    {},
    { alias: 'appAuthorization' }
  );

  readonly viewContainerRef = inject(ViewContainerRef);
  readonly templateRef = inject(TemplateRef<unknown>);

  readonly authService = inject(AuthService);

  constructor() {
    effect(() => {
      const isAuthenticated = this.authService.isAuthenticated();
      this.authorize(isAuthenticated);
    });
  }

  ngOnInit() {
    const isAuthenticated = this.authService.isAuthenticated();

    this.authorize(isAuthenticated);
  }

  checkRoles(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      return false;
    }

    const requiredRoles = this.authorizationParams()?.roles || [];
    const userRoles = this.authService.user()?.roles || [];

    const hasAnyRequiredRole = requiredRoles.some((requiredRole) => {
      return userRoles.some((userRole) => {
        return (
          userRole.name === requiredRole.name &&
          (!requiredRole.value || userRole.value === requiredRole.value)
        );
      });
    });

    return hasAnyRequiredRole;
  }

  checkOperationClaims(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      return false;
    }

    const requiredOperationClaims =
      this.authorizationParams()?.operationClaims || [];
    const userOperationClaims = this.authService.user()?.operationClaims || [];

    const hasAnyRequiredOperationClaim = requiredOperationClaims.some(
      (requiredOperationClaim) => {
        return userOperationClaims.some((userOperationClaim) => {
          return (
            userOperationClaim.name === requiredOperationClaim.name &&
            userOperationClaim.value === requiredOperationClaim.value
          );
        });
      }
    );

    return hasAnyRequiredOperationClaim;
  }

  authorize(isAuthenticated: boolean) {
    const hasAnyTrueFlag = this.checkFlags();

    if (hasAnyTrueFlag || this.checkRolesOrOperationClaims(isAuthenticated)) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

  private checkFlags() {
    const flags = this.authorizationParams().flags || [];

    debugger;

    return flags.some((flag) => flag);
  }

  private checkRolesOrOperationClaims(isAuthenticated: boolean) {
    if (!isAuthenticated) {
      return false;
    }

    return (
      this.checkRoles(isAuthenticated) ||
      this.checkOperationClaims(isAuthenticated)
    );
  }
}

export type AuthorizationParams = {
  roles?: UserRoleDto[];
  operationClaims?: UserOperationClaimDto[];
  flags?: boolean[];
};

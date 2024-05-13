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

  checkRoles() {
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

  checkOperationClaims() {
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
    this.viewContainerRef.clear();
    if (!isAuthenticated) {
      return;
    }

    const hasAnyTrueFlag = this.checkFlags();
    const hasAnyRequiredRoleOrOperationClaim =
      this.checkRolesOrOperationClaims();

    if (hasAnyTrueFlag || hasAnyRequiredRoleOrOperationClaim) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }

  private checkFlags() {
    const flags = this.authorizationParams().flags || [];

    return flags.some((flag) => flag);
  }

  private checkRolesOrOperationClaims() {
    return this.checkRoles() || this.checkOperationClaims();
  }
}

export type AuthorizationParams = {
  roles?: UserRoleDto[];
  operationClaims?: UserOperationClaimDto[];
  flags?: boolean[];
};

import { UserOperationClaimDto } from './operation-claim';
import { UserRoleDto } from './role';

export type UserClaimsDto = {
  roles: UserRoleDto[];
  operationClaims: UserOperationClaimDto[];
};

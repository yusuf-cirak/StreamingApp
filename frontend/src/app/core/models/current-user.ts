import { UserOperationClaimDto } from '../modules/auths/models/operation-claim';
import { UserRoleDto } from '../modules/auths/models/role';
import { User } from './user';

export interface CurrentUser extends User {
  accessToken: string;
  tokenExpiration: Date;
  refreshToken: string;
  roles: UserRoleDto[];
  operationClaims: UserOperationClaimDto[];
}

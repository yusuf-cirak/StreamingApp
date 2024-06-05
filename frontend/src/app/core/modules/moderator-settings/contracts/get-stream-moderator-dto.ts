import { User } from '../../../models';

export interface GetStreamModeratorDto extends User {
  roleIds: string[];
  operationClaimIds: string[];
}

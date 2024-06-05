import { inject, Injectable } from '@angular/core';
import { HttpClientService } from '../../../../shared/services/http-client.service';
import { GetRoleDto } from '../contracts/get-role-dto';
import { GetOperationClaimDto } from '../contracts/get-operation-claim-dto';

@Injectable({
  providedIn: 'root',
})
export class PermissionProxyService {
  private readonly httpClientService = inject(HttpClientService);

  getStreamRoles() {
    return this.httpClientService.get<GetRoleDto[]>({
      controller: 'roles',
      action: 'stream',
    });
  }

  getStreamOperationClaims() {
    return this.httpClientService.get<GetOperationClaimDto[]>({
      controller: 'operation-claims',
      action: 'stream',
    });
  }
}

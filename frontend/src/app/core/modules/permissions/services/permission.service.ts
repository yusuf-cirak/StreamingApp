import { inject, Injectable } from '@angular/core';
import { PermissionProxyService } from './permission-proxy.service';
import { toSignal } from '@angular/core/rxjs-interop';

@Injectable()
export class PermissionService {
  private readonly permissionProxyService = inject(PermissionProxyService);
  streamRoles = toSignal(this.permissionProxyService.getStreamRoles());
  streamOperationClaims = toSignal(
    this.permissionProxyService.getStreamOperationClaims()
  );
}

import { TestBed } from '@angular/core/testing';

import { PermissionProxyService } from './permission-proxy.service';

describe('PermissionProxyService', () => {
  let service: PermissionProxyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PermissionProxyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

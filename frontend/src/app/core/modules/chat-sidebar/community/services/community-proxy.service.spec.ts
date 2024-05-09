import { TestBed } from '@angular/core/testing';

import { CommunityProxyService } from './community-proxy.service';

describe('CommunityProxyService', () => {
  let service: CommunityProxyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommunityProxyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

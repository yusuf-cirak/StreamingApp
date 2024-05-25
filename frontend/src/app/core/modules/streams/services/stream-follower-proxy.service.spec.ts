import { TestBed } from '@angular/core/testing';

import { StreamFollowerProxyService } from './stream-follower-proxy.service';

describe('StreamFollowerProxyService', () => {
  let service: StreamFollowerProxyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StreamFollowerProxyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { StreamOptionProxyService } from './stream-option-proxy.service';

describe('StreamOptionProxyService', () => {
  let service: StreamOptionProxyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StreamOptionProxyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

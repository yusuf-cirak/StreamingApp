import { TestBed } from '@angular/core/testing';

import { ModeratorSearchProxyService } from './moderator-search-proxy.service';

describe('ModeratorSearchProxyService', () => {
  let service: ModeratorSearchProxyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModeratorSearchProxyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

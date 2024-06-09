import { TestBed } from '@angular/core/testing';

import { UserAuthorizationService } from './user-authorization.service';

describe('UserAuthorizationService', () => {
  let service: UserAuthorizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserAuthorizationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

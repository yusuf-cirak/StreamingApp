import { TestBed } from '@angular/core/testing';

import { CommunityBlockService } from './community-block.service';

describe('CommunityBlockService', () => {
  let service: CommunityBlockService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommunityBlockService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

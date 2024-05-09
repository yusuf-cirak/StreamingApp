import { TestBed } from '@angular/core/testing';

import { CommunityViewService } from './community-view.service';

describe('CommunityViewService', () => {
  let service: CommunityViewService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommunityViewService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { StreamFollowerService } from './stream-follower.service';

describe('StreamFollowerService', () => {
  let service: StreamFollowerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StreamFollowerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

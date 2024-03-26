import { TestBed } from '@angular/core/testing';

import { UserImageService } from './user-image.service';

describe('UserImageService', () => {
  let service: UserImageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserImageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

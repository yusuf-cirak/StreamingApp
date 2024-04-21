import { TestBed } from '@angular/core/testing';

import { LocalStorageEventService } from './local-storage-event.service';

describe('LocalStorageEventService', () => {
  let service: LocalStorageEventService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LocalStorageEventService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

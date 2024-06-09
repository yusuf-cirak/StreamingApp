import { TestBed } from '@angular/core/testing';

import { ModeratorSettingsService } from './moderator-settings.service';

describe('ModeratorSettingsService', () => {
  let service: ModeratorSettingsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModeratorSettingsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

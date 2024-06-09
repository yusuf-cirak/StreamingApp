import { TestBed } from '@angular/core/testing';

import { ChatSettingsService } from './chat-settings.service';

describe('ChatSettingsService', () => {
  let service: ChatSettingsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatSettingsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

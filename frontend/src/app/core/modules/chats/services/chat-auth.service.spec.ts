import { TestBed } from '@angular/core/testing';

import { ChatAuthService } from './chat-auth.service';

describe('ChatAuthService', () => {
  let service: ChatAuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatAuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

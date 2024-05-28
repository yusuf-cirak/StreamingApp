import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { streamerIdResolver } from './streamer-id.resolver';

describe('streamerIdResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => streamerIdResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});

import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { AuthService } from '../../../services';

export const streamerIdResolver: ResolveFn<string> = (route, state) => {
  return route.paramMap.get('streamerId') ?? inject(AuthService).userId()!;
};

import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { Observable } from 'rxjs';
import { UserLoginDto } from '../../../dtos/user-login-dto';
import { UserAuthDto } from '../dtos/user-auth-dto';
import { UserRefreshTokenDto } from '../../../dtos/user-refresh-token-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthProxyService {
  readonly httpClientService = inject(HttpClientService);

  login(loginDto: UserLoginDto): Observable<UserAuthDto> {
    return this.httpClientService.post(
      { controller: 'auths', action: 'login' },
      loginDto
    );
  }

  register(loginDto: UserLoginDto): Observable<UserAuthDto> {
    return this.httpClientService.post(
      { controller: 'auths', action: 'register' },
      loginDto
    );
  }

  refreshToken(refreshTokenDto: UserRefreshTokenDto): Observable<UserAuthDto> {
    return this.httpClientService.post(
      { controller: 'auths', action: 'refresh' },
      refreshTokenDto
    );
  }

  followingStreamIds(): Observable<string[]> {
    return this.httpClientService.get({
      controller: 'stream-follower-users',
    });
  }
}

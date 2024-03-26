import { Injectable, inject } from '@angular/core';
import { User } from '../models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserImageService {
  getProfileImageUrl(user?: User): string {
    const url = `${environment.defaultProfileImageApiUrl}/?background=a0a0a0&name=`;
    if (!user) {
      return url;
    }

    return user.profileImageUrl || `${url}${user.username}`;
  }
}

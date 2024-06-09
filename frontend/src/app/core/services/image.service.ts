import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CurrentUser } from '../models/current-user';
import { User } from '../models';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  getThumbnailUrl(thumbnail: string): string {
    return `${environment.cloudinary.baseUrl}/stream_thumbnails/${thumbnail}`;
  }

  getProfilePictureUrl(user: CurrentUser | User | undefined): string {
    return user
      ? user.profileImageUrl
        ? `${environment.cloudinary.baseUrl}/${environment.cloudinary.folderNames.profileImages}/${user.profileImageUrl}`
        : `${environment.defaultProfileImageApiUrl}/?background=a0a0a0&name=${user.username}`
      : '';
  }

  getDefaultProfilePictureUrl(username: string): string {
    return `${environment.defaultProfileImageApiUrl}/?background=a0a0a0&name=${username}`;
  }
}

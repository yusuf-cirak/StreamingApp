import { inject, Injectable } from '@angular/core';
import { StreamBlockUserDto } from '../../models/stream-block-user-dto';
import { CommunityProxyService } from './community-proxy.service';

@Injectable()
export class CommunityBlockService {
  private readonly communityProxyService = inject(CommunityProxyService);

  isBlocked(streamerId: string) {
    return this.communityProxyService.isUserBlocked(streamerId);
  }
  block(blockedUserDto: StreamBlockUserDto) {
    return this.communityProxyService.blockUser(blockedUserDto);
  }

  unblock(blockedUserDto: StreamBlockUserDto) {
    return this.communityProxyService.unblockUser(blockedUserDto);
  }
}

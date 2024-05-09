import { inject, Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { StreamBlockUserDto } from '../../models/stream-block-user-dto';
import { CommunityProxyService } from './community-proxy.service';

@Injectable()
export class CommunityBlockService {
  private readonly communityProxyService = inject(CommunityProxyService);

  block(blockedUserDto: StreamBlockUserDto) {
    return this.communityProxyService.blockUser(blockedUserDto);
  }

  unblock(blockedUserDto: StreamBlockUserDto) {
    return this.communityProxyService.unblockUser(blockedUserDto);
  }
}

import { computed, inject, Injectable, signal } from '@angular/core';
import {
  getRequiredReadOperationClaims,
  getRequiredWriteOperationClaims,
  OperationClaims,
} from '../../../constants/operation-claims';
import { getRequiredRoles } from '../../../constants/roles';
import { UserOperationClaimDto } from '../../../modules/auths/models/operation-claim';
import { CurrentCreatorService } from './current-creator-service';
import { AuthService } from '@streaming-app/core';
import { CreatorPage } from '../guards/creator-page.guard';

@Injectable({ providedIn: 'root' })
export class CreatorAuthService {
  private readonly creatorService = inject(CurrentCreatorService);
  private readonly authService = inject(AuthService);

  readonly isStreamerFlag = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;
    const userId = this.authService.userId();

    return streamerId === userId;
  });

  private readonly creatorPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;

    const roles = getRequiredRoles(streamerId);

    const operationClaims = getRequiredReadOperationClaims(streamerId).concat(
      getRequiredWriteOperationClaims(streamerId)
    );

    return {
      roles,
      operationClaims,
      flags: [this.isStreamerFlag()],
    };
  });
  private readonly keyPageRequirements = computed(() => {
    return {
      flags: [this.isStreamerFlag()],
    };
  });
  readonly moderatorPageRequirements = computed(() => {
    return {
      flags: [this.isStreamerFlag()],
    };
  });

  private readonly chatSettingsPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;
    const roles = getRequiredRoles(streamerId);

    const operationClaims: UserOperationClaimDto[] = [
      { name: OperationClaims.Stream.Read.ChatOptions, value: streamerId },
      { name: OperationClaims.Stream.Write.ChatOptions, value: streamerId },
    ];

    return {
      roles,
      operationClaims,
      flags: [this.isStreamerFlag()],
    };
  });

  private readonly communityPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;
    const roles = getRequiredRoles(streamerId);

    const operationClaims: UserOperationClaimDto[] = [
      { name: OperationClaims.Stream.Read.BlockFromChat, value: streamerId },
      { name: OperationClaims.Stream.Write.BlockFromChat, value: streamerId },
    ];

    return {
      roles,
      operationClaims,
      flags: [this.isStreamerFlag()],
    };
  });

  readonly pageRequirement = {
    get: (page: CreatorPage) => {
      switch (page) {
        case 'creator':
          return this.creatorPageRequirements();
        case 'key':
          return this.keyPageRequirements();
        case 'moderators':
          return this.moderatorPageRequirements();
        case 'chat-settings':
          return this.chatSettingsPageRequirements();
        case 'community':
          return this.communityPageRequirements();

        default:
          return { flags: [] };
      }
    },
  };
}

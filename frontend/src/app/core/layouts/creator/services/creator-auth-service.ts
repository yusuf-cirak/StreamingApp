import { computed, inject, Injectable, signal } from '@angular/core';
import { OperationClaims } from '../../../constants/operation-claims';
import { getRequiredRoles } from '../../../constants/roles';
import { UserOperationClaimDto } from '../../../modules/auths/models/operation-claim';
import { CurrentCreatorService } from './current-creator-service';

@Injectable({ providedIn: 'root' })
export class CreatorAuthService {
  private readonly creatorService = inject(CurrentCreatorService);

  readonly creatorPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;

    const roles = getRequiredRoles(streamerId);

    return {
      roles,
    };
  });
  readonly keyPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;

    const roles = getRequiredRoles(streamerId).filter(
      (role) => role.name === 'Streamer'
    );

    return {
      roles,
    };
  });

  readonly chatSettingsPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;
    const roles = getRequiredRoles(streamerId);

    const operationClaims: UserOperationClaimDto[] = [
      { name: OperationClaims.Stream.Read.ChatOptions, value: streamerId },
      { name: OperationClaims.Stream.Write.ChatOptions, value: streamerId },
    ];

    return {
      roles,
      operationClaims,
    };
  });

  readonly communityPageRequirements = computed(() => {
    const streamerId = this.creatorService.streamer()?.id as string;
    const roles = getRequiredRoles(streamerId);

    const operationClaims: UserOperationClaimDto[] = [
      { name: OperationClaims.Stream.Read.BlockFromChat, value: streamerId },
      { name: OperationClaims.Stream.Write.BlockFromChat, value: streamerId },
    ];

    return {
      roles,
      operationClaims,
    };
  });
}

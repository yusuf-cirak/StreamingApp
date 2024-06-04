import { inject, Injectable } from '@angular/core';
import { StreamOptionProxyService } from './proxy/stream-option-proxy.service';
import { Subject, tap } from 'rxjs';
import { PatchStreamChatSettingsDto } from '../dtos/patch-stream-chat-settings-dto';
import { PatchStreamTitleDescriptionDto } from '../dtos/patch-stream-title-description-dto';

@Injectable({
  providedIn: 'root',
})
export class ChatSettingsService {
  private readonly chatSettingsProxyService = inject(StreamOptionProxyService);

  readonly patchChatSettings$ = new Subject<PatchStreamChatSettingsDto>();
  readonly patchStreamTitleDescription$ =
    new Subject<PatchStreamTitleDescriptionDto>();

  getChatSettings(streamerId: string) {
    return this.chatSettingsProxyService.getChatSettings(streamerId);
  }

  patchChatSettings(chatSettingsDto: PatchStreamChatSettingsDto) {
    return this.chatSettingsProxyService
      .patchChatSettings(chatSettingsDto)
      .pipe(tap(() => this.patchChatSettings$.next(chatSettingsDto)));
  }

  patchStreamTitleDescription(
    chatTitleDescriptionDto: PatchStreamTitleDescriptionDto
  ) {
    return this.chatSettingsProxyService
      .patchTitleDescription(chatTitleDescriptionDto)
      .pipe(
        tap(() =>
          this.patchStreamTitleDescription$.next(chatTitleDescriptionDto)
        )
      );
  }
}

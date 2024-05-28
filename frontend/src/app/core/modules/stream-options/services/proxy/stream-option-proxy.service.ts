import { Injectable, inject } from '@angular/core';
import { HttpClientService } from '@streaming-app/shared/services';
import { PatchStreamChatSettingsDto } from '../../dtos/patch-stream-chat-settings-dto';
import { PatchStreamTitleDescriptionDto } from '../../dtos/patch-stream-title-description-dto';
import { AuthService } from '@streaming-app/core';
import { GetChatSettingsDto } from '../../dtos/get-chat-settings-dto';

@Injectable({
  providedIn: 'root',
})
export class StreamOptionProxyService {
  private readonly httpClient = inject(HttpClientService);

  private readonly authService = inject(AuthService);

  getChatSettings(streamerId: string) {
    return this.httpClient.get<GetChatSettingsDto>({
      controller: 'stream-options',
      action: 'chat-settings',
      routeParams: [streamerId],
    });
  }

  patchChatSettings(chatSettingsDto: PatchStreamChatSettingsDto) {
    return this.httpClient.patch(
      {
        controller: 'stream-options',
        action: 'chat-settings',
      },
      chatSettingsDto
    );
  }

  getTitleDescription(streamerId: string) {
    return this.httpClient.get(
      {
        controller: 'stream-options',
        action: 'title-description',
      },
      streamerId
    );
  }

  patchTitleDescription(
    chatTitleDescriptionDto: PatchStreamTitleDescriptionDto
  ) {
    return this.httpClient.patch(
      {
        controller: 'stream-options',
        action: 'title-description',
      },
      chatTitleDescriptionDto
    );
  }

  getStreamKey(streamerId: string) {
    return this.httpClient.get(
      {
        controller: 'stream-options',
        action: 'key',
      },
      streamerId
    );
  }

  generateStreamKey() {
    const userId = this.authService.user()?.id;
    return this.httpClient.post(
      {
        controller: 'stream-options',
        action: 'key',
      },
      { streamerId: userId }
    );
  }
}

import { Component, inject } from '@angular/core';
import { NgIf, NgStyle } from '@angular/common';
import { CreatorNavbarComponent } from './components/navbar/creator-navbar.component';
import { CreatorSidebarComponent } from './components/sidebar/creator-sidebar.component';
import { RouterOutlet } from '@angular/router';
import { CreatorService } from './services/creator-service';
import { StreamFacade } from '../../modules/streams/services/stream.facade';
import { ChatSettingsService } from '../../modules/stream-options/services/chat-settings.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';
import { PermissionService } from '../../modules/permissions/services/permission.service';

@Component({
  selector: 'app-creator',
  standalone: true,
  imports: [
    CreatorNavbarComponent,
    CreatorSidebarComponent,
    NgStyle,
    NgIf,
    RouterOutlet,
  ],
  providers: [CreatorService, PermissionService],
  templateUrl: './creator.layout.html',
})
export class CreatorLayout {
  readonly streamerFacade = inject(StreamFacade);
  readonly chatSettingsService = inject(ChatSettingsService);

  constructor() {
    this.listenToChatSettingEvents();
  }

  private listenToChatSettingEvents() {
    this.chatSettingsService.patchChatSettings$
      .pipe(
        takeUntilDestroyed(),
        tap((chatSettings) => {
          this.streamerFacade.setStreamChatOptions(chatSettings);
        })
      )
      .subscribe();

    this.chatSettingsService.patchStreamTitleDescription$
      .pipe(
        takeUntilDestroyed(),
        tap((titleDescription) => {
          const stream = this.streamerFacade.stream();
          const streamOptions = { ...stream.streamOption, ...titleDescription };
          this.streamerFacade.setStreamOptions(streamOptions);
        })
      )
      .subscribe();
  }
}

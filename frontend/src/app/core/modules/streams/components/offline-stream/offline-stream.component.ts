import { Component, input } from '@angular/core';
import { ChatSidebarComponent } from '../../../chat-sidebar/chat-sidebar.component';
import { Error } from '../../../../../shared/api/error';
import { OfflineStreamerIcon } from '../../../../../shared/icons/offline-streamer-icon';

@Component({
  selector: 'app-offline-stream',
  standalone: true,
  templateUrl: './offline-stream.component.html',
  imports: [ChatSidebarComponent,OfflineStreamerIcon],
})
export class OfflineStreamComponent {
  streamError = input.required<Error>();

}

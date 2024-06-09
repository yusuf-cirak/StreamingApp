import { Component, inject } from '@angular/core';
import { ChatMessageComponent } from '../chat-message/chat-message.component';
import { StreamFacade } from '../../../../streams/services/stream.facade';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatMessageComponent],
  templateUrl: './chat-list.component.html',
})
export class ChatListComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly messages = this.streamFacade.chatMessages;
}

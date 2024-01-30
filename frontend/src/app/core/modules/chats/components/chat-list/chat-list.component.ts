import { Component, input } from '@angular/core';
import { ChatMessage } from '../../models/chat-message';
import { ChatMessageComponent } from '../chat-message/chat-message.component';

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatMessageComponent],
  templateUrl: './chat-list.component.html',
})
export class ChatListComponent {
  messages = input.required<ChatMessage[]>();
}

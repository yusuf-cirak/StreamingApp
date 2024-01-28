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
  // messages = input.required<ChatMessage[]>();
  messages: ChatMessage[] = [
    { message: 'deneme3', sentAt: new Date(), username: 'yusuf' },
    { message: 'deneme7', sentAt: new Date(), username: 'yusuf' },
    { message: 'deneme00', sentAt: new Date(), username: 'yusuf' },
    { message: 'deneme00', sentAt: new Date(), username: 'yusuf' },
    { message: 'deneme000', sentAt: new Date(), username: 'yusuf' },
    {
      message:
        'deneme000ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd',
      sentAt: new Date(),
      username: 'yusuf',
    },
    {
      message:
        'deneme000ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd',
      sentAt: new Date(),
      username: 'yusuf',
    },
    {
      message:
        'deneme000ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd',
      sentAt: new Date(),
      username: 'yusuf',
    },
    {
      message:
        'deneme000ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd',
      sentAt: new Date(),
      username: 'yusuf',
    },
  ].reverse();
}

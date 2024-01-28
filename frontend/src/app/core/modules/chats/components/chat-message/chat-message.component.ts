import { Component, input } from '@angular/core';
import { ChatMessage } from '../../models/chat-message';
import { DatePipe, NgStyle } from '@angular/common';
import { stringToColor } from '../../../../../shared/utils/string-to-color';

@Component({
  selector: 'app-chat-message',
  standalone: true,
  imports: [DatePipe, NgStyle],
  templateUrl: './chat-message.component.html',
})
export class ChatMessageComponent {
  data = input.required<ChatMessage>();

  usernameColor(username: string) {
    return stringToColor(username);
  }
}

import { Component, input } from '@angular/core';
import { ChatMessage } from '../../models/chat-message';
import { DatePipe, NgStyle } from '@angular/common';
import { TextHashColorDirective } from '@streaming-app/shared/directives';

@Component({
  selector: 'app-chat-message',
  standalone: true,
  imports: [DatePipe, NgStyle, TextHashColorDirective],
  templateUrl: './chat-message.component.html',
})
export class ChatMessageComponent {
  data = input.required<ChatMessage>();
}

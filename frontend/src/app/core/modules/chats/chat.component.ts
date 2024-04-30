import {
  Component,
  EventEmitter,
  Output,
  computed,
  inject,
  input,
  signal,
} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { ChatDisabledIcon } from '../../../shared/icons/chat-disabled.icon';
import { ChatListComponent } from './components/chat-list/chat-list.component';
import { ChatFormComponent } from './components/chat-form/chat-form.component';
import { ChatAuthService } from './services/chat-auth.service';
import { LiveStreamDto } from '../recommended-streamers/models/live-stream-dto';
import { ChatMessage } from './models/chat-message';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    NgTemplateOutlet,
    ChatDisabledIcon,
    ChatListComponent,
    ChatFormComponent,
  ],
  templateUrl: './chat.component.html',
})
export class ChatComponent {
  liveStream = input.required<LiveStreamDto>();

  chatMessages = input.required<ChatMessage[]>();

  readonly chatAuthService = inject(ChatAuthService);

  @Output() messageSend = new EventEmitter<string>();

  onMessageSend(message: string) {
    this.messageSend.emit(message);
  }
}

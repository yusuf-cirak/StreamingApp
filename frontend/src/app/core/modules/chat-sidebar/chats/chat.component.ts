import {
  Component,
  EventEmitter,
  Output,
  computed,
  inject,
  input,
} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { ChatListComponent } from './components/chat-list/chat-list.component';
import { ChatFormComponent } from './components/chat-form/chat-form.component';
import { ChatAuthService } from './services/chat-auth.service';
import { ChatMessage } from './models/chat-message';
import { ChatDisabledIcon } from '../../../../shared/icons/chat-disabled.icon';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from '../../streams/services/stream.facade';

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
  readonly streamFacade = inject(StreamFacade);

  readonly chatAuthService = inject(ChatAuthService);

  @Output() messageSend = new EventEmitter<string>();

  onMessageSend(message: string) {
    this.messageSend.emit(message);
  }
}

import { Component, input } from '@angular/core';
import { ChatState } from './models/chat-state';
import { NgTemplateOutlet } from '@angular/common';
import { ChatDisabledIcon } from '../../../shared/icons/chat-disabled.icon';
import { ChatListComponent } from './components/chat-list/chat-list.component';
import { ChatFormComponent } from './components/chat-form/chat-form.component';

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
  chatState = input.required<ChatState>();
}

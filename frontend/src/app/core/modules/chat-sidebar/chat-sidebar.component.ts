import { Component, EventEmitter, Output, input, signal } from '@angular/core';
import { ChatSidebar } from './models/chat-sidebar';
import { CommunityIconComponent } from '../../../shared/icons/community-icon';
import { ChatIconComponent } from '../../../shared/icons/chat-icon';
import { TooltipModule } from 'primeng/tooltip';
import { ChatComponent } from '../chats/chat.component';
import { LiveStreamDto } from '../recommended-streamers/models/live-stream-dto';

@Component({
  selector: 'app-chat-sidebar',
  standalone: true,
  imports: [
    CommunityIconComponent,
    ChatIconComponent,
    TooltipModule,
    ChatComponent,
  ],
  templateUrl: './chat-sidebar.component.html',
})
export class ChatSidebarComponent {
  liveStream = input.required<LiveStreamDto>();


  chatSidebar = signal<ChatSidebar>({
    variant: 'chat',
  });

  @Output() messageSend = new EventEmitter<string>();

  changeVariant() {
    const currentVariant = this.chatSidebar().variant;
    const newVariant = currentVariant === 'chat' ? 'community' : 'chat';
    this.chatSidebar.update(() => ({ variant: newVariant }));
  }


  onMessageSend(message: string) {
    this.messageSend.emit(message);
  }
}

import { Component, signal } from '@angular/core';
import { ChatSidebar } from './models/chat-sidebar';
import { CommunityIconComponent } from '../../../shared/icons/community-icon';
import { ChatIconComponent } from '../../../shared/icons/chat-icon';
import { TooltipModule } from 'primeng/tooltip';
import { ChatComponent } from '../chats/chat.component';
import { ChatState } from '../chats/models/chat-state';

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
  chatSidebar = signal<ChatSidebar>({
    variant: 'chat',
  });

  chatState = signal<ChatState>({
    enabled: true,
    delaySecond: 0,
    followersOnly: true,
  });

  changeVariant() {
    const currentVariant = this.chatSidebar().variant;
    const newVariant = currentVariant === 'chat' ? 'community' : 'chat';
    this.chatSidebar.update(() => ({ variant: newVariant }));
  }
}

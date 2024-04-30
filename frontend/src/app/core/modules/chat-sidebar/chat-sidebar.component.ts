import { Component, inject, input, Signal, signal } from '@angular/core';
import { ChatSidebar } from './models/chat-sidebar';
import { CommunityIconComponent } from '../../../shared/icons/community-icon';
import { ChatIconComponent } from '../../../shared/icons/chat-icon';
import { TooltipModule } from 'primeng/tooltip';
import { ChatComponent } from '../chats/chat.component';
import { LiveStreamDto } from '../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from '../streams/services/stream.facade';

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
  readonly streamFacade = inject(StreamFacade);

  readonly liveStream = this.streamFacade.liveStream as Signal<LiveStreamDto>;

  readonly chatMessages = this.streamFacade.chatMessages;

  chatSidebar = signal<ChatSidebar>({
    variant: 'chat',
  });

  changeVariant() {
    const currentVariant = this.chatSidebar().variant;
    const newVariant = currentVariant === 'chat' ? 'community' : 'chat';
    this.chatSidebar.update(() => ({ variant: newVariant }));
  }

  onMessageSend(message: string) {
    this.streamFacade.sendMessage(message);
  }
}

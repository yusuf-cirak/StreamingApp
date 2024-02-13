import { User } from '@streaming-app/core';
import { StreamOptions } from '../../../models/stream-options';
import { ChatMessage } from '../../chats/models/chat-message';

export interface LiveStreamDto {
  startedAt: Date;
  user: User;
  options: StreamOptions;
  chatMessages: ChatMessage[];
}

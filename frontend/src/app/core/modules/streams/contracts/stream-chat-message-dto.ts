import { User } from '../../../models';

export type StreamChatMessageDto = {
  sender: User;
  message: string;
};

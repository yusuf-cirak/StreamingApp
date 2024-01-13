import { Entity } from '@streaming-app/core';
export interface Streamer extends Entity {
  id: string;
  streamId: string;
  username: string;
  imageUrl: string;
  isLive: boolean;
}

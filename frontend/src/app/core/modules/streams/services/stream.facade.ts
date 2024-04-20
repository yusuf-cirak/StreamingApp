import { Injectable, computed, inject, signal } from '@angular/core';
import { StreamService } from './stream.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { StreamState } from '../models/stream-state';
import { Error } from '../../../../shared/api/error';
import { AuthService } from '@streaming-app/core';
import { StreamHub } from '../../../hubs/stream-hub';
import { StreamChatOptionsDto } from '../contracts/stream-options-dto';

@Injectable({ providedIn: 'root' })
export class StreamFacade {
  #streamError = signal<Error | undefined>(undefined);
  #liveStream = signal<LiveStreamDto | undefined>(undefined);

  streamState = computed(() => ({
    stream: this.#liveStream(),
    error: this.#streamError(),
  }));

  #streamerName = signal<string>('');

  readonly liveStream = computed(() => this.streamState()?.stream);

  readonly streamerName = computed(
    () => this.liveStream()?.user.username || this.#streamerName()
  );

  readonly authService = inject(AuthService);

  readonly streamService = inject(StreamService);

  readonly streamHub = inject(StreamHub);

  getHlsUrl() {
    return this.streamService.getHlsUrl(
      this.liveStream()?.options.streamKey || ''
    );
  }

  setLiveStream(liveStream: LiveStreamDto | undefined) {
    this.#liveStream.set(liveStream);
  }
  setStreamChatOptions(chatOptions: StreamChatOptionsDto) {
    // this.streamState.update((state) => {
    //   const stream = state?.stream;
    //   const options = { ...stream?.options, ...chatOptions };
    //   return {
    //     ...state,
    //     stream: { ...stream, options: { ...options, chatOptions: options } },
    //   } as StreamState;
    // });

    this.#liveStream.update((stream) => {
      const options = { ...stream?.options, ...chatOptions };
      return {
        ...stream,
        options: { ...options, chatOptions: options },
      } as LiveStreamDto;
    });
  }

  setLiveStreamErrorState(error: Error) {
    this.#streamError.set(error);
  }

  endStream() {
    this.#liveStream.set(undefined);
    this.#streamError.set({
      statusCode: 400,
      message: 'Stream is now offline',
    });
  }

  leaveStream() {
    this.#liveStream.set(undefined);
  }

  setStreamerName(streamerName: string) {
    this.#streamerName.set(streamerName);
  }

  sendMessage(message: string) {
    // send message to the server
    const liveStream = this.liveStream();
    const user = this.authService.user();

    const messages = liveStream?.chatMessages || [];

    liveStream!.chatMessages = [
      { message, sentAt: new Date(), username: user?.username! },
      ...messages,
    ];

    this.setLiveStream(liveStream!);
  }

  connectToStreamHub() {
    this.streamHub.connect();
  }

  joinStreamRoom(userId: string) {
    this.streamHub.invokeOnJoinedStream(userId);
  }

  leaveStreamRoom(userId: string) {
    this.streamHub.invokeOnLeavedStream(userId);
  }
}

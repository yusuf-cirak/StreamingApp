import { Injectable, computed, inject, signal } from '@angular/core';
import { StreamService } from './stream.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { Error } from '../../../../shared/api/error';
import { AuthService } from '@streaming-app/core';
import { StreamHub } from '../../../hubs/stream-hub';
import { StreamChatOptionsDto } from '../contracts/stream-options-dto';
import { Observable, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { StreamChatMessageDto } from '../contracts/stream-chat-message-dto';
import { ChatMessage } from '../../chat-sidebar/chats/models/chat-message';

@Injectable({ providedIn: 'root' })
export class StreamFacade {
  // services
  readonly authService = inject(AuthService);

  readonly streamService = inject(StreamService);

  readonly streamHub = inject(StreamHub);

  // states

  #streamError = signal<Error | undefined>(undefined);
  #liveStream = signal<LiveStreamDto | undefined>(undefined);
  #chatMessages = signal<ChatMessage[]>([]);

  streamState = computed(() => ({
    stream: this.#liveStream(),
    error: this.#streamError(),
  }));

  #streamerName = signal<string>('');

  readonly liveStream = computed(() => this.streamState()?.stream);

  readonly streamerName = computed(
    () => this.liveStream()?.user.username || this.#streamerName()
  );

  readonly chatMessages = this.#chatMessages.asReadonly();

  // sources

  readonly joinStream$ = new Subject<Observable<unknown>>();
  readonly leaveStream$ = new Subject<Observable<unknown>>();

  constructor() {
    // reducers
    this.joinStream$
      .pipe(
        takeUntilDestroyed(),
        switchMap((source) => source)
      )
      .subscribe();

    this.leaveStream$
      .pipe(
        takeUntilDestroyed(),
        switchMap((source) => source)
      )
      .subscribe();

    // effects
    this.streamHub.streamChatMessageReceived$
      .pipe(
        takeUntilDestroyed(),
        tap((chatMessageDto) => {
          this.#chatMessages.update((messages) => {
            return [
              {
                message: chatMessageDto.message,
                sentAt: new Date(),
                username: chatMessageDto.sender.username,
              },
              ...messages,
            ];
          });
        })
      )
      .subscribe();
  }

  getHlsUrl() {
    return this.streamService.getHlsUrl(
      this.liveStream()?.options.streamKey || ''
    );
  }

  setLiveStream(liveStream: LiveStreamDto | undefined) {
    this.#liveStream.update(() => liveStream);
  }
  setStreamChatOptions(chatOptions: StreamChatOptionsDto) {
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
    this.#chatMessages.set([]);
    this.#streamError.set(undefined);
  }

  joinStreamRoom(userId: string) {
    this.joinStream$.next(this.streamHub.invokeOnJoinedStream(userId));
  }

  leaveStreamRoom(userId: string) {
    this.leaveStream$.next(this.streamHub.invokeOnLeavedStream(userId));
  }

  setStreamerName(streamerName: string) {
    this.#streamerName.set(streamerName);
  }

  sendMessage(message: string) {
    // send message to the server
    const streamerName = this.streamerName();
    const streamChatMessageDto = {
      sender: this.authService.currentUserToUserDto(),
      message,
    } as StreamChatMessageDto;

    return this.streamHub.invokeOnStreamChatMessageSend(
      streamerName,
      streamChatMessageDto
    );
  }
}

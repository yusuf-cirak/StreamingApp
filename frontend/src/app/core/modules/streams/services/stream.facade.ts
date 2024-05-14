import { Injectable, Signal, computed, inject, signal } from '@angular/core';
import { StreamService } from './stream.service';
import { Error } from '../../../../shared/api/error';
import { AuthService } from '@streaming-app/core';
import { StreamHub } from '../../../hubs/stream-hub';
import { StreamChatOptionsDto } from '../contracts/stream-options-dto';
import { Observable, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { StreamChatMessageDto } from '../contracts/stream-chat-message-dto';
import { ChatMessage } from '../../chat-sidebar/chats/models/chat-message';
import { StreamState } from '../models/stream-state';
import { StreamOptions } from '../../../models/stream-options';
import { StreamDto } from '../contracts/stream-dto';

@Injectable({ providedIn: 'root' })
export class StreamFacade {
  // services
  readonly authService = inject(AuthService);

  readonly streamService = inject(StreamService);

  readonly streamHub = inject(StreamHub);

  // states

  #streamState = signal<StreamState | undefined>(undefined);
  #chatMessages = signal<ChatMessage[]>([]);

  #streamerName = signal<string>('');

  readonly streamState = this.#streamState.asReadonly();

  readonly liveStream = computed(
    () => this.#streamState()?.stream
  ) as Signal<StreamDto>;

  readonly isStreamLive = computed(
    () => this.streamState()?.error?.statusCode !== 400 && !!this.liveStream()
  );

  readonly isStreamerExists = computed(
    () => !!this.liveStream() || this.streamState()?.error?.statusCode !== 404
  );

  readonly streamerName = computed(
    () => this.liveStream()?.user.username || this.#streamerName()
  );

  readonly chatMessages = this.#chatMessages.asReadonly();

  // todo: do this in streamAuthService
  readonly isFollowingLiveStreamer = computed(() => {
    const followingStreamers = this.authService.followingStreamIds();

    const following = followingStreamers.includes(this.liveStream().user.id);
    return following;
  });

  readonly canJoinToChatRoom = computed(
    () => this.streamHub.connectedToHub() && this.isStreamerExists()
  );

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
      this.liveStream()?.streamOption?.streamKey || ''
    );
  }

  setStream(liveStream: StreamState | undefined) {
    this.#streamState.update(() => liveStream);
  }

  updateStream(streamDto: StreamDto) {
    this.#streamState.set({
      stream: streamDto,
      error: null,
    });
  }
  setStreamChatOptions(chatOptions: StreamChatOptionsDto) {
    const streamState = this.#streamState() as StreamState;
    const stream = streamState.stream;
    const newOptions = {
      ...stream?.streamOption,
      ...chatOptions,
    } as StreamOptions;

    this.#streamState.set({
      stream: { ...stream, streamOption: newOptions } as StreamDto,
      error: null,
    });
  }

  setLiveStreamErrorState(error: Error) {
    this.#streamState.update((state) => {
      return {
        ...state,
        error,
      } as StreamState;
    });
  }

  endStream() {
    this.#streamState.update((state) => {
      return {
        ...state,
        error: {
          statusCode: 400,
          message: 'Stream is now offline',
        },
      } as StreamState;
    });
  }

  leaveStream() {
    this.#streamState.set(undefined);
    this.#chatMessages.set([]);
  }

  joinStreamRoom(streamerName: string) {
    this.joinStream$.next(this.streamHub.invokeOnJoinedStream(streamerName));
  }

  leaveStreamRoom(streamerName: string) {
    this.leaveStream$.next(this.streamHub.invokeOnLeavedStream(streamerName));
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

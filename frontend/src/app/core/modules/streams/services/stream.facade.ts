import { Injectable, Signal, computed, inject, signal } from '@angular/core';
import { StreamService } from './stream.service';
import { Error } from '../../../../shared/api/error';
import { AuthService, User } from '@streaming-app/core';
import { StreamHub } from '../../../hubs/stream-hub';
import { Observable, Subject, switchMap, tap } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { StreamChatMessageDto } from '../../../hubs/dtos/stream-chat-message-dto';
import { ChatMessage } from '../../chat-sidebar/chats/models/chat-message';
import { StreamState } from '../models/stream-state';
import { StreamOptions } from '../../../models/stream-options';
import { StreamDto } from '../contracts/stream-dto';
import { UserAuthorizationService } from '../../../services/user-authorization.service';
import { getRequiredRoles } from '../../../constants/roles';
import { getRequiredWriteOperationClaims } from '../../../constants/operation-claims';

@Injectable({ providedIn: 'root' })
export class StreamFacade {
  // services
  private readonly authService = inject(AuthService);

  private readonly streamService = inject(StreamService);

  private readonly streamHub = inject(StreamHub);

  private readonly userAuthorizationService = inject(UserAuthorizationService);

  // states

  #streamer = signal<User | null>(null);

  #streamOptions = signal<StreamOptions | null>(null);
  readonly streamOptions = this.#streamOptions.asReadonly();

  stream = computed(() => ({
    user: this.#streamer(),
    streamOption: this.#streamOptions(),
  })) as Signal<StreamDto>;

  #error = signal<Error | null>(null);

  #chatMessages = signal<ChatMessage[]>([]);

  #streamerName = signal<string>('');

  streamState = computed(() => ({
    stream: this.stream(),
    error: this.#error(),
  })) as Signal<StreamState>;

  readonly liveStream = computed(
    () => this.streamState()?.stream
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

  // readonly isHost = computed(() => {
  //   return (
  //     this.liveStream()?.user.username === this.authService.user()?.username
  //   );
  // });

  readonly isAuthorized = computed(() => {
    const streamerId = this.liveStream()?.user.id;

    const authorizeOptions = {
      roles: getRequiredRoles(streamerId),
      operationClaims: getRequiredWriteOperationClaims(streamerId),
      flags: [streamerId == this.authService.userId()],
    };

    return this.userAuthorizationService.check(authorizeOptions);
  });

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

    this.streamHub.streamBlockUserOccured$
      .pipe(
        takeUntilDestroyed(),
        tap((blockUserDto) => {
          const blockedStreamerIds = this.authService.blockedStreamIds();
          const updatedBlockedStreamers = blockUserDto.isBlocked
            ? [...blockedStreamerIds, blockUserDto.streamerId]
            : blockedStreamerIds.filter((id) => id !== blockUserDto.streamerId);
          this.authService.updateBlockedStreamers(updatedBlockedStreamers);
        })
      )
      .subscribe();
  }

  getHlsUrl() {
    return this.streamService.getHlsUrl(
      this.liveStream()?.streamOption?.streamKey || ''
    );
  }

  setStream(streamState: StreamState | undefined) {
    this.#streamer.set(streamState?.stream?.user || null);
    this.#streamOptions.set(streamState?.stream?.streamOption || null);

    this.#error.set(streamState?.error || null);
  }

  updateStream(streamDto: StreamDto) {
    this.#streamer.update(() => streamDto.user);
    this.#streamOptions.update(() => streamDto.streamOption);

    this.#error.set(null);
  }
  setStreamChatOptions(chatOptions: StreamOptions) {
    const streamState = this.streamState() as StreamState;
    const stream = streamState.stream;
    const newOptions = {
      ...stream?.streamOption,
      ...chatOptions,
    } as StreamOptions;

    this.#streamOptions.set(newOptions);
    // this.#error.set(null);
  }

  setStreamOptions(streamOptions: StreamOptions) {
    this.#streamOptions.set(streamOptions);
  }

  setLiveStreamErrorState(error: Error) {
    this.#error.set(error);
  }

  endStream() {
    this.#error.set({
      statusCode: 400,
      message: 'Stream is now offline',
    });
  }

  clearChatMessages() {
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

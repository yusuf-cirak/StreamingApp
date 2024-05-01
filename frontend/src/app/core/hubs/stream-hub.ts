import { inject, Injectable, signal, Signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { catchError, EMPTY, from, Subject, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StreamHubAction } from './stream-hub-action';
import { LiveStreamDto } from '../modules/recommended-streamers/models/live-stream-dto';
import { StreamInfoDto } from '../modules/streams/contracts/stream-info-dto';
import { StreamChatOptionsDto } from '../modules/streams/contracts/stream-options-dto';
import { StreamChatMessageDto } from '../modules/streams/contracts/stream-chat-message-dto';

@Injectable({ providedIn: 'root' })
export class StreamHub {
  private _hubConnection!: signalR.HubConnection;

  readonly connectedToHub = signal(false);

  streamStarted$ = new Subject<LiveStreamDto>();
  streamEnd$ = new Subject<string>();

  streamChatOptionsChanged$ = new Subject<StreamChatOptionsDto>();

  streamChatMessageReceived$ = new Subject<StreamChatMessageDto>();

  connect() {
    return from(this._hubConnection.start()).pipe(
      tap(() => {
        console.log('Connected to stream hub');
        this.registerStreamHubHandlers();
        this.connectedToHub.set(true);
        console.log('Registered stream hub handlers');
      }),
      catchError((err) => {
        console.error(err);
        return EMPTY;
      })
    );
  }

  disconnect() {
    return from(this._hubConnection.stop()).pipe(
      tap(() => {
        console.log('Disconnected from stream hub');
        this.connectedToHub.set(false);
      }),
      catchError((err) => {
        console.error(err);
        return EMPTY;
      })
    );
  }

  registerStreamHubHandlers() {
    this._hubConnection.on(
      StreamHubAction.OnStreamStarted,
      (streamInfo: StreamInfoDto) => {
        const stream = {
          startedAt: streamInfo.startedAt,
          options: streamInfo.streamOption,
          user: streamInfo.user,
        } as LiveStreamDto;

        this.streamStarted$.next(stream);
      }
    );

    this._hubConnection.on(
      StreamHubAction.OnStreamEnd,
      (streamerName: string) => {
        this.streamEnd$.next(streamerName);
      }
    );

    this._hubConnection.on(
      StreamHubAction.OnStreamChatOptionsChanged,
      (streamChatOptionsDto: StreamChatOptionsDto) => {
        this.streamChatOptionsChanged$.next(streamChatOptionsDto);
      }
    );

    this._hubConnection.on(
      StreamHubAction.OnStreamChatMessageSend,
      (streamChatMessageDto: StreamChatMessageDto) => {
        this.streamChatMessageReceived$.next(streamChatMessageDto);
      }
    );
  }

  invokeOnJoinedStream(streamerId: string) {
    return from(
      this._hubConnection.invoke(StreamHubAction.OnJoinedStream, streamerId)
    ).pipe(
      tap(() => console.log('Joined stream')),
      catchError((err) => {
        console.error(err);
        return EMPTY;
      })
    );
  }

  invokeOnLeavedStream(streamerId: string) {
    return from(
      this._hubConnection.invoke(StreamHubAction.OnLeavedStream, streamerId)
    ).pipe(
      tap(() => console.log('Leaved stream')),
      catchError((err) => {
        console.error(err);
        return EMPTY;
      })
    );
  }

  invokeOnStreamChatMessageSend(
    streamerName: string,
    streamChatMessageDto: StreamChatMessageDto
  ) {
    return from(
      this._hubConnection.invoke(
        StreamHubAction.OnStreamChatMessageSend,
        streamerName,
        streamChatMessageDto
      )
    ).pipe(
      tap(() => console.log('Message sent')),
      catchError((err) => {
        console.error(err);
        return err;
      })
    );
  }

  buildAndConnect(accessToken?: string) {
    this.buildConnection(accessToken);
    return this.connect();
  }

  private buildConnection(accessToken?: string) {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.streamHubUrl!, {
        accessTokenFactory: () => accessToken!,
      })
      .build();
  }
}

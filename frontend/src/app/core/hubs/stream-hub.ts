import { inject, Injectable, signal, Signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { catchError, EMPTY, from, Subject, tap, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StreamHubAction } from './stream-hub-action';
import { StreamDto } from '../modules/streams/contracts/stream-dto';
import { StreamInfoDto } from './dtos/stream-info-dto';
import { StreamChatOptionsDto } from './dtos/stream-options-dto';
import { StreamChatMessageDto } from './dtos/stream-chat-message-dto';
import { StreamBlockUserActionDto } from './dtos/stream-block-user-action-dto';

@Injectable({ providedIn: 'root' })
export class StreamHub {
  private _hubConnection!: signalR.HubConnection;

  readonly connectedToHub = signal(false);

  streamStarted$ = new Subject<StreamDto>();
  streamEnd$ = new Subject<string>();

  streamChatOptionsChanged$ = new Subject<StreamChatOptionsDto>();

  streamChatMessageReceived$ = new Subject<StreamChatMessageDto>();

  streamBlockUserOccured$ = new Subject<StreamBlockUserActionDto>();

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
        this.connectedToHub.set(false);

        return throwError(err);
      })
    );
  }

  disconnect() {
    return from(this._hubConnection.stop()).pipe(
      tap(() => {
        this.connectedToHub.set(false);
        console.log('Disconnected from stream hub');
      }),
      catchError((err) => {
        console.error(err);
        return throwError(err);
      })
    );
  }

  registerStreamHubHandlers() {
    this._hubConnection.on(
      StreamHubAction.OnStreamStarted,
      (streamInfo: StreamInfoDto) => {
        const stream = {
          startedAt: streamInfo.startedAt,
          streamOption: streamInfo.streamOption,
          user: streamInfo.user,
        } as StreamDto;

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

    this._hubConnection.on(
      StreamHubAction.OnBlockFromStream,
      (streamBlockUserDto: StreamBlockUserActionDto) =>
        this.streamBlockUserOccured$.next(streamBlockUserDto)
    );
  }

  invokeOnJoinedStream(streamerId: string) {
    return from(
      this._hubConnection.invoke(StreamHubAction.OnJoinedStream, streamerId)
    ).pipe(
      tap(() => {
        console.log('Joined stream');
      }),
      catchError((err) => {
        console.error(err);
        return throwError(err);
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
        return throwError(err);
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
        return throwError(err);
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

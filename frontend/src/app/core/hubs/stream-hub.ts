import { inject, Injectable, Signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { catchError, EMPTY, from, Subject, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { StreamHubAction } from './stream-hub-action';
import { LiveStreamDto } from '../modules/recommended-streamers/models/live-stream-dto';
import { StreamInfoDto } from '../modules/streams/contracts/stream-info-dto';
import { StreamChatOptionsDto } from '../modules/streams/contracts/stream-options-dto';

@Injectable({ providedIn: 'root' })
export class StreamHub {
  private _hubConnection!: signalR.HubConnection;

  streamStarted$ = new Subject<LiveStreamDto>();
  streamEnd$ = new Subject<string>();

  streamChatOptionsChanged$ = new Subject<StreamChatOptionsDto>();

  connect() {
    return from(this._hubConnection.start()).pipe(
      tap(() => {
        console.log('Connected to stream hub');
        this.registerStreamHubHandlers();
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
      tap(() => console.log('Disconnected from stream hub')),
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

import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../services';
import { StreamHubAction } from './stream-hub-action';
import { LiveStreamDto } from '../modules/recommended-streamers/models/live-stream-dto';
import { StreamInfoDto } from '../modules/streams/contracts/stream-info-dto';
import { StreamChatOptionsDto } from '../modules/streams/contracts/stream-options-dto';

@Injectable({ providedIn: 'root' })
export class StreamHub {
  private readonly authService = inject(AuthService);
  private readonly _hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.streamHubUrl!, {
      accessTokenFactory: () => this.authService.user()?.accessToken!,
    })
    .build();

  streamStarted$ = new Subject<LiveStreamDto>();
  streamEnd$ = new Subject<string>();

  streamChatOptionsChanged$ = new Subject<StreamChatOptionsDto>();

  async connect() {
    await this._hubConnection
      .start()
      .then(() => {
        console.log('Connected to stream hub');
        this.registerStreamHubHandlers();
        console.log('Registered stream hub handlers');
      })
      .catch((err) => {
        console.error(err);
      });
  }

  async disconnect() {
    await this._hubConnection
      .stop()
      .then(() => {
        console.log('Disconnected from stream hub');
      })
      .catch((err) => {
        console.error(err);
      });
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
    this._hubConnection
      .invoke(StreamHubAction.OnJoinedStream, streamerId)
      .then(() => {
        console.log('Joined stream');
      })
      .catch((err) => {
        console.error(err);
      });
  }

  invokeOnLeavedStream(streamerId: string) {
    this._hubConnection
      .invoke(StreamHubAction.OnLeavedStream, streamerId)
      .then(() => {
        console.log('Leaved stream');
      })
      .catch((err) => {
        console.error(err);
      });
  }
}

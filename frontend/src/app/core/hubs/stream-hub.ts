import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../services';
import { StreamHubAction } from './stream-hub-action';

@Injectable({ providedIn: 'root' })
export class StreamHub {
  private readonly authService = inject(AuthService);
  private readonly _hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.streamHubUrl!, {
      accessTokenFactory: () => this.authService.user()?.accessToken!,
    })
    .build();

  // chatMessageReceived$ = new BehaviorSubject<MessageDto>(null!);
  // chatGroupCreated$ = new BehaviorSubject<CreateHubChatGroupDto>(null!);

  connect() {
    this._hubConnection
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

  disconnect() {
    this._hubConnection
      .stop()
      .then(() => {
        console.log('Disconnected from stream hub');
      })
      .catch((err) => {
        console.error(err);
      });
  }

  registerStreamHubHandlers() {
    // this._hubConnection.on('ReceiveMessageAsync', (message: MessageDto) => {
    //   this.chatMessageReceived$.next(message);
    // });
    // this._hubConnection.on(
    //   'ChatGroupCreatedAsync',
    //   (chatGroup: CreateHubChatGroupDto) => {
    //     this.chatGroupCreated$.next(chatGroup);
    //   }
    // );
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

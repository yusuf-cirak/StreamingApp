import { Injectable, computed, inject, signal } from '@angular/core';
import { StreamService } from './stream.service';
import { LiveStreamDto } from '../../recommended-streamers/models/live-stream-dto';
import { StreamState } from '../models/stream-state';
import { Error } from '../../../../shared/api/error';
import { AuthService } from '@streaming-app/core';

@Injectable({providedIn:'root'})
export class StreamFacade {
  #streamState = signal<StreamState | undefined>(undefined)

  readonly authService = inject(AuthService);

  readonly streamState = computed(() => this.#streamState());

  readonly liveStream = computed(() => this.#streamState()?.stream);

  readonly streamService = inject(StreamService);

  setLiveStream(liveStream: LiveStreamDto) {
    this.#streamState.update((state) =>({...state,stream:liveStream}));
  }

  setLiveStreamErrorState(error: Error) {
    this.#streamState.update((state) =>({...state,error}));
  }


  getHlsUrl() {
    return this.streamService.getHlsUrl(this.liveStream()?.options.streamKey || '');
  }

  sendMessage(message:string){
    // send message to the server
    const liveStream = this.liveStream();
    const user = this.authService.user();

    const messages = liveStream?.chatMessages || [];

    liveStream!.chatMessages =
     [
      { message, sentAt: new Date(), username: user?.username! },
      ...messages,
    ];

    this.setLiveStream(liveStream!);
  }
}

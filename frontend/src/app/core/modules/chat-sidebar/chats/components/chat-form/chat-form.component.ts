import {
  Component,
  HostListener,
  Signal,
  computed,
  inject,
  signal,
} from '@angular/core';
import { RippleModule } from 'primeng/ripple';
import { NgClass } from '@angular/common';
import { ChatAuthService } from '../../services/chat-auth.service';
import {
  catchError,
  delay,
  EMPTY,
  filter,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HintComponent } from '../../../../../components/hint/hint.component';
import { AuthService } from '../../../../../services';
import { LiveStreamDto } from '../../../../recommended-streamers/models/live-stream-dto';
import { StreamFacade } from '../../../../streams/services/stream.facade';

@Component({
  selector: 'app-chat-form',
  standalone: true,
  imports: [RippleModule, NgClass, HintComponent],
  templateUrl: './chat-form.component.html',
})
export class ChatFormComponent {
  readonly authService = inject(AuthService);

  readonly chatAuthService = inject(ChatAuthService);

  readonly streamFacade = inject(StreamFacade);

  readonly liveStream = this.streamFacade.liveStream as Signal<LiveStreamDto>;

  chatOptions = computed(() => this.liveStream().options);

  chatDisabled = computed(() => this.chatOptions().chatDisabled);

  canSendMessage = computed(() => {
    return (
      !this.chatDisabled() &&
      !this.chatAuthService.canUserSendMessage(this.liveStream())
    );
  });

  readonly chatHintMessage = computed(
    () =>
      this.chatAuthService.canUserSendMessage(this.liveStream()) ??
      this.chatAuthService.chatDelayMessage() ??
      ''
  );

  sendingMessage = signal(false);

  message = signal<string>('');

  messageSent$ = new Subject<Observable<any>>();

  @HostListener('document:keydown.enter', ['$event'])
  onEnter() {
    this.messageSent$.next(this.sendMessage());
  }

  constructor() {
    this.messageSent$
      .pipe(
        takeUntilDestroyed(),
        switchMap((source) => source)
      )
      .subscribe();
  }

  setMessage(message: string) {
    this.message.update(() => message);
  }

  sendMessage() {
    const message = this.message();
    const chatDelaySecond = this.chatOptions().chatDelaySecond;
    return of(message).pipe(
      filter((message) => !!message),
      tap(() => {
        this.sendingMessage.set(chatDelaySecond > 0);
      }),
      delay(chatDelaySecond * 1000),
      switchMap(() => this.streamFacade.sendMessage(this.message())),
      tap(() => {
        this.message.update(() => '');
        this.sendingMessage.set(false);
      }),
      catchError(() => {
        this.message.update(() => '');
        this.sendingMessage.set(false);
        return EMPTY;
      })
    );
  }
}

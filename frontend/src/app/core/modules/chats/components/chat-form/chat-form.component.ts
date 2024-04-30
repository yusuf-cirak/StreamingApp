import {
  Component,
  HostListener,
  computed,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { RippleModule } from 'primeng/ripple';
import { NgClass } from '@angular/common';
import { HintComponent } from '../../../../components/hint/hint.component';
import { AuthService } from '../../../../services';
import { LiveStreamDto } from '../../../recommended-streamers/models/live-stream-dto';
import { ChatAuthService } from '../../services/chat-auth.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-chat-form',
  standalone: true,
  imports: [RippleModule, NgClass, HintComponent],
  templateUrl: './chat-form.component.html',
})
export class ChatFormComponent {
  liveStream = input.required<LiveStreamDto>();

  chatOptions = computed(() => this.liveStream().options);

  chatDisabled = computed(() => this.chatOptions().chatDisabled);

  canSendMessage = computed(() => {
    return (
      !this.chatDisabled() &&
      !this.chatAuthService.canUserSendMessage(this.liveStream())
    );
  });

  sendingMessage = signal(false);

  readonly authService = inject(AuthService);

  readonly chatAuthService = inject(ChatAuthService);

  readonly chatHintMessage = computed(
    () =>
      this.chatAuthService.canUserSendMessage(this.liveStream()) ??
      this.chatAuthService.chatDelayMessage() ??
      ''
  );

  message = signal<string>('');

  messageSend = output<string>();

  @HostListener('document:keydown.enter', ['$event'])
  onEnter() {
    this.sendMessage();
  }

  setMessage(message: string) {
    this.message.update(() => message);
  }

  sendMessage() {
    const message = this.message();
    if (!this.chatDisabled() && message) {
      const chatDelaySecond = this.chatOptions().chatDelaySecond;
      this.sendingMessage.set(chatDelaySecond > 0);
      setTimeout(() => {
        this.messageSend.emit(this.message());
        this.message.update(() => '');
        this.sendingMessage.set(false);
      }, chatDelaySecond * 1000);
    }
  }
}

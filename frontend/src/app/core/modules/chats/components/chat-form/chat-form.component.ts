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

  readonly authService = inject(AuthService);

  readonly chatAuthService = inject(ChatAuthService);

  readonly userChatErrorMessage = computed(() =>
    this.chatAuthService.canUserSendMessage(this.liveStream())
  );

  message = signal<string>('');

  messageSend = output<string>();

  @HostListener('document:keydown.enter', ['$event'])
  onEnter() {
    const chatOptions = this.chatOptions();
    const message = this.message();
    if (!chatOptions.chatDisabled && message) {
      setTimeout(() => {
        this.sendMessage();
      }, chatOptions.chatDelaySecond * 1000);
    }
  }

  setMessage(message: string) {
    this.message.update(() => message);
  }

  sendMessage() {
    if (!this.chatDisabled()) {
      this.messageSend.emit(this.message());
      this.message.update(() => '');
    }
  }
}

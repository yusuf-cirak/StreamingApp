import {
  Component,
  EventEmitter,
  HostListener,
  Output,
  inject,
  input,
  signal,
} from '@angular/core';
import { StreamState } from '../../../streams/models/stream-state';
import { RippleModule } from 'primeng/ripple';
import { NgClass } from '@angular/common';
import { HintComponent } from '../../../../components/hint/hint.component';
import { AuthService } from '../../../../services';

@Component({
  selector: 'app-chat-form',
  standalone: true,
  imports: [RippleModule, NgClass, HintComponent],
  templateUrl: './chat-form.component.html',
})
export class ChatFormComponent {
  streamState = input.required<StreamState>();

  readonly authService = inject(AuthService);

  message = signal<string>('');

  @Output() messageSend = new EventEmitter<string>();

  @HostListener('document:keydown.enter', ['$event'])
  onEnter() {
    const streamState = this.streamState();
    const message = this.message();
    if (streamState.enabled && message) {
      setTimeout(() => {
        this.sendMessage();
      }, streamState.delaySecond * 1000);
    }
  }

  setMessage(message: string) {
    this.message.update(() => message);
  }

  sendMessage() {
    this.messageSend.emit(this.message());
    this.message.update(() => '');
  }
}

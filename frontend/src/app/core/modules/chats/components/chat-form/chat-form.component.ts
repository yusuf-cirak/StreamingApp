import { Component, HostListener, input, signal } from '@angular/core';
import { ChatState } from '../../models/chat-state';
import { RippleModule } from 'primeng/ripple';
import { NgClass } from '@angular/common';
import { HintComponent } from '../../../../components/hint/hint.component';

@Component({
  selector: 'app-chat-form',
  standalone: true,
  imports: [RippleModule, NgClass, HintComponent],
  templateUrl: './chat-form.component.html',
})
export class ChatFormComponent {
  chatState = input.required<ChatState>();

  @HostListener('document:keydown.enter', ['$event'])
  onEnter() {
    const chatState = this.chatState();
    const message = this.message();
    if (chatState.enabled && message) {
      setTimeout(() => {
        this.sendMessage();
      }, chatState.delaySecond * 1000);
    }
  }

  message = signal<string>('');

  setMessage(message: string) {
    this.message.update(() => message);
  }

  sendMessage() {}
}

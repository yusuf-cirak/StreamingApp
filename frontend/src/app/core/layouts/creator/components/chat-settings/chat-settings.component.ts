import { Component, computed, signal } from '@angular/core';
import {
  BrnSwitchComponent,
  BrnSwitchThumbComponent,
} from '@spartan-ng/ui-switch-brain';
import {
  HlmSwitchDirective,
  HlmSwitchThumbDirective,
} from '@spartan-ng/ui-switch-helm';

import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
@Component({
  standalone: true,
  selector: 'app-chat-settings',
  templateUrl: './chat-settings.component.html',
  imports: [
    HlmSwitchDirective,
    BrnSwitchComponent,
    BrnSwitchThumbComponent,
    HlmSwitchThumbDirective,
    HlmInputDirective,
  ],
})
export class ChatSettingsComponent {
  #chatEnabled = signal<boolean | undefined>(undefined);
  chatEnabled = this.#chatEnabled.asReadonly();

  ngOnInit() {
    this.#chatEnabled.set(true);
  }

  setChatEnabled(value: boolean) {
    this.#chatEnabled.set(value);
  }
}

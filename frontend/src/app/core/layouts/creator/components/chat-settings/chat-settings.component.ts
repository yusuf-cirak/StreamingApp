import { Component, computed, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { NonNullableFormBuilder } from '@angular/forms';
import { min, max } from '../../../../validators';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
@Component({
  standalone: true,
  selector: 'app-chat-settings',
  templateUrl: './chat-settings.component.html',
  imports: [InputSwitchModule, InputNumberModule, RippleModule, ButtonModule],
})
export class ChatSettingsComponent {
  readonly fb = inject(NonNullableFormBuilder);

  readonly form = this.fb.group({
    chatEnabled: [true, []],
    mustBeFollower: [false, []],
    chatDelaySeconds: [
      0,
      [
        min(0, 'Chat delay must be greater than 0'),
        max(60, 'Chat delay must be less than 60'),
      ],
    ],
  });

  //TODO: Update form values
}

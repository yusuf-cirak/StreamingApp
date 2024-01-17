import { Component, DestroyRef, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { min, max } from '../../../../validators';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ChatSettingsSkeletonComponent } from './skeleton/chat-settings-skeleton.component';
@Component({
  standalone: true,
  selector: 'app-chat-settings',
  templateUrl: './chat-settings.component.html',
  imports: [
    InputSwitchModule,
    InputNumberModule,
    RippleModule,
    ButtonModule,
    ReactiveFormsModule,
    ChatSettingsSkeletonComponent,
  ],
})
export class ChatSettingsComponent {
  readonly destroyRef = inject(DestroyRef);
  readonly fb = inject(NonNullableFormBuilder);

  readonly #loaded = signal<boolean>(false);
  readonly loaded = this.#loaded.asReadonly();

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

  ngOnInit() {
    this.form.controls.chatEnabled.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((value: boolean) => {
        const chatDelaySecondsControl = this.form.controls.chatDelaySeconds;
        if (value) {
          chatDelaySecondsControl.enable();
        } else {
          chatDelaySecondsControl.setValue(0);
          chatDelaySecondsControl.disable();
        }
      });
  }

  updateChatSettings(valid: boolean, value: typeof this.form.value) {
    if (!valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
  }

  //TODO: Update form values
}

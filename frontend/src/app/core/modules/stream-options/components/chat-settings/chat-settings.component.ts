import { tap } from 'rxjs';
import { Component, DestroyRef, effect, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { min, max } from '../../../../validators';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { ChatSettingsSkeletonComponent } from './skeleton/chat-settings-skeleton.component';
import { StreamOptionProxyService } from '../../services/proxy/stream-option-proxy.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '@streaming-app/core';
import { finalize } from 'rxjs';
import { PatchStreamChatSettingsDto } from '../../dtos/patch-stream-chat-settings-dto';
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
  providers: [StreamOptionProxyService],
})
export class ChatSettingsComponent {
  readonly destroyRef = inject(DestroyRef);
  readonly fb = inject(NonNullableFormBuilder);
  readonly route = inject(ActivatedRoute);

  readonly #loaded = signal<boolean>(true);
  readonly loaded = this.#loaded.asReadonly();

  readonly streamOptionProxyService = inject(StreamOptionProxyService);
  readonly authService = inject(AuthService);

  readonly form = this.fb.group({
    chatEnabled: [true, []],
    mustBeFollower: [false, []],
    chatDelaySecond: [
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
      .subscribe(() => {
        this.updateFormValues();
      });

    this.streamOptionProxyService
      .getChatSettings(this.authService.userId()!)
      .pipe(finalize(() => this.#loaded.set(true)))
      .subscribe({
        next: (settings) => {
          this.form.patchValue({
            ...settings,
            chatEnabled: !settings.chatDisabled,
          }),
            this.updateFormValues();
        },
      });
  }

  patchChatSettings(valid: boolean, value: typeof this.form.value) {
    if (!valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
    const { chatEnabled, ...rest } = value;

    const formValues = {
      ...rest,
      chatDisabled: !chatEnabled,
      streamerId: this.authService.userId()!,
    } as PatchStreamChatSettingsDto;

    this.streamOptionProxyService
      .patchChatSettings(formValues)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.form.enable();
          this.updateFormValues();
        })
      )
      .subscribe();
  }

  updateFormValues() {
    const chatDelaySeconds = this.form.controls.chatDelaySecond;
    if (this.form.controls.chatEnabled.value) {
      chatDelaySeconds.enable();
    } else {
      chatDelaySeconds.setValue(0);
      chatDelaySeconds.disable();
    }
  }
}

import {
  catchError,
  EMPTY,
  exhaustMap,
  filter,
  merge,
  Observable,
  of,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { Component, DestroyRef, inject, Signal, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { min, max } from '../../../../validators';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import {
  takeUntilDestroyed,
  toObservable,
  toSignal,
} from '@angular/core/rxjs-interop';
import { ChatSettingsSkeletonComponent } from './skeleton/chat-settings-skeleton.component';
import { StreamOptionProxyService } from '../../services/proxy/stream-option-proxy.service';
import { AuthService } from '@streaming-app/core';
import { finalize } from 'rxjs';
import { PatchStreamChatSettingsDto } from '../../dtos/patch-stream-chat-settings-dto';
import { ToastrService } from 'ngx-toastr';
import { CurrentCreatorService } from '../../../../layouts/creator/services/current-creator-service';
import { ChatSettingsService } from '../../services/chat-settings.service';
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
  readonly toastr = inject(ToastrService);
  readonly destroyRef = inject(DestroyRef);
  readonly fb = inject(NonNullableFormBuilder);

  readonly currentCreatorService = inject(CurrentCreatorService);

  readonly streamerId = this.currentCreatorService.streamerId as Signal<string>;

  readonly #loaded = signal(false);
  readonly loaded = this.#loaded.asReadonly();

  readonly chatSettingsService = inject(ChatSettingsService);

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

  readonly update$ = new Subject<Observable<unknown>>();

  readonly chatSettings = toSignal(
    merge(toObservable(this.streamerId), this.update$).pipe(
      catchError(() => of([])),
      filter(Boolean),
      switchMap((source) => source),
      exhaustMap(() =>
        this.chatSettingsService.getChatSettings(this.streamerId())
      ),
      tap((chatSettings) => {
        this.#loaded.set(true);

        this.form.patchValue({
          ...chatSettings,
          chatEnabled: !chatSettings.chatDisabled,
        }),
          this.updateFormValues();
      })
    )
  );

  ngOnInit() {
    this.form.controls.chatEnabled.valueChanges
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.updateFormValues();
      });
  }

  patchChatSettings(valid: boolean, value: typeof this.form.value) {
    if (!this.form.touched) {
      return;
    }
    if (!valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
    const { chatEnabled, ...rest } = value;
    const formValues = {
      ...rest,
      chatDisabled: !chatEnabled,
      streamerId: this.streamerId(),
    } as PatchStreamChatSettingsDto;

    const update$ = this.chatSettingsService.patchChatSettings(formValues).pipe(
      takeUntilDestroyed(this.destroyRef),
      catchError((error) => {
        this.toastr.error(
          error?.error?.message || 'Something went wrong',
          'Error'
        );
        return EMPTY;
      }),
      tap(() => {
        this.toastr.success('Chat settings updated', 'Success');
      }),
      finalize(() => {
        this.form.enable();
        this.form.markAsUntouched();
        this.updateFormValues();
      })
    );

    this.update$.next(update$);
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

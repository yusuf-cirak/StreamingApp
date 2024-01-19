import { Component, DestroyRef, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { min, max } from '../../../../validators';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { InputTextModule } from 'primeng/inputtext';
import { CopyClipboardComponent } from '../../../../components/copy-clipboard/copy-clipboard.component';
import { KeySkeletonComponent } from './skeleton/key-skeleton.component';
@Component({
  standalone: true,
  selector: 'app-key',
  templateUrl: './key.component.html',
  imports: [
    InputSwitchModule,
    InputNumberModule,
    RippleModule,
    ButtonModule,
    ReactiveFormsModule,
    KeySkeletonComponent,
    InputTextModule,
    CopyClipboardComponent,
  ],
})
export class KeyComponent {
  readonly #loaded = signal<boolean>(false);
  readonly loaded = this.#loaded.asReadonly();

  //TODO: Update form values
}

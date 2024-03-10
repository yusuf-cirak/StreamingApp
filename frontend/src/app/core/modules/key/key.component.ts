import { Component, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { ReactiveFormsModule } from '@angular/forms';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { KeySkeletonComponent } from './skeleton/key-skeleton.component';
import { CopyClipboardComponent } from '../../components/copy-clipboard/copy-clipboard.component';
import { KeyService } from './services/key.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { tap } from 'rxjs';
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

  readonly keyService = inject(KeyService);
  readonly key = toSignal(
    this.keyService.get().pipe(
      tap(() => {
        this.#loaded.set(true);
      })
    )
  );
}

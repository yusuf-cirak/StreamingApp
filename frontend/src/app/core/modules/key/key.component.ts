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
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { InfoIcon } from '../../../shared/icons/info-icon';
import { environment } from '../../../../environments/environment';

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
    InfoIcon,
  ],
})
export class KeyComponent {
  readonly #loaded = signal(false);
  readonly loaded = this.#loaded.asReadonly();

  readonly rtmpUrl = environment.rtmpUrl;

  readonly #generateKeySubmitted = signal(false);
  readonly generateKeySubmitted = this.#generateKeySubmitted.asReadonly();

  readonly keyService = inject(KeyService);

  readonly key$ = new BehaviorSubject<Observable<string>>(this.getStreamKey());

  readonly key = toSignal(
    this.key$.pipe(switchMap((observable) => observable))
  );

  generateStreamKey() {
    this.#generateKeySubmitted.set(true);
    this.key$.next(
      this.keyService.generate().pipe(
        tap(() => {
          this.#generateKeySubmitted.set(false);
        })
      )
    );
  }

  getStreamKey() {
    return this.keyService.get().pipe(tap(() => this.#loaded.set(true)));
  }
}

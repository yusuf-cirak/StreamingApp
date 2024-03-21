import { Component, DestroyRef, inject, signal } from '@angular/core';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { ReactiveFormsModule } from '@angular/forms';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { KeySkeletonComponent } from './skeleton/key-skeleton.component';
import { CopyClipboardComponent } from '../../components/copy-clipboard/copy-clipboard.component';
import { KeyService } from './services/key.service';
import {
  takeUntilDestroyed,
  toObservable,
  toSignal,
} from '@angular/core/rxjs-interop';
import {
  BehaviorSubject,
  concatMap,
  lastValueFrom,
  Observable,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { InfoIcon } from '../../../shared/icons/info-icon';

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
  readonly #destroyRef = inject(DestroyRef);
  readonly #loaded = signal(false);
  readonly loaded = this.#loaded.asReadonly();

  readonly #generateKeySubmitted = signal(false);
  readonly generateKeySubmitted = this.#generateKeySubmitted.asReadonly();

  // readonly key$ = new Subject<Observable<string>>();

  readonly keyService = inject(KeyService);

  readonly #key = signal<string>('');
  readonly key = this.#key.asReadonly();

  ngOnInit() {
    this.getStreamKey();
  }

  generateStreamKey() {
    this.#generateKeySubmitted.set(true);

    this.keyService
      .generate()
      .pipe(
        takeUntilDestroyed(this.#destroyRef),
        tap((value) => {
          this.#generateKeySubmitted.set(false);
          this.#key.set(value);
        })
      )
      .subscribe();
  }

  getStreamKey() {
    this.keyService
      .get()
      .pipe(
        takeUntilDestroyed(this.#destroyRef),
        tap((value) => {
          this.#loaded.set(true);
          this.#key.set(value);
        })
      )
      .subscribe();
  }
}

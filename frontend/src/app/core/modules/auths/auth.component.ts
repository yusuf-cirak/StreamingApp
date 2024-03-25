import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  EventEmitter,
  Output,
  inject,
  input,
  signal,
} from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TabViewModule } from 'primeng/tabview';
import { AuthService } from '@streaming-app/core';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { minLength } from '../../components/validators/min-length';
import { UserLoginDto } from '../../dtos/user-login-dto';
import { InputComponent } from '../../../shared/components/input/input.component';
import { RippleModule } from 'primeng/ripple';
import { LoadingIcon } from '../../../shared/icons/loading-icon';
import { ModalComponent } from '../../../shared/components/modal/modal.component';
import { TabPanelComponent } from '../../../shared/components/tab-panel/tab-panel.component';
import { UserRegisterDto } from '../../dtos/user-register-dto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ToastrService } from 'ngx-toastr';
import { Tab } from './models/tab';
import { UserAuthDto } from './dtos/user-auth-dto';
import { tap } from 'rxjs';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    ButtonModule,
    DialogModule,
    TabViewModule,
    ReactiveFormsModule,
    InputComponent,
    RippleModule,
    LoadingIcon,
    ModalComponent,
    TabPanelComponent,
  ],
  templateUrl: './auth.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthComponent {
  visible = input.required<boolean>();

  #destroyRef = inject(DestroyRef);
  private readonly toastrService = inject(ToastrService);

  #tabs = signal<Tab[]>(['Login', 'Register']);
  tabs = this.#tabs.asReadonly();

  #activeTab = signal('Login');
  activeTab = this.#activeTab.asReadonly();

  @Output() closeModalClicked = new EventEmitter<void>();

  @Output() outsideModalClicked = new EventEmitter<void>();

  readonly authService = inject(AuthService);

  readonly fb = inject(NonNullableFormBuilder);

  readonly form = this.fb.group({
    username: ['', [minLength(4, 'User name')]],
    password: ['', [minLength(4, 'Password')]],
  });

  #formSubmitted = signal(false);
  formSubmitted = this.#formSubmitted.asReadonly();

  emitCloseModalClicked() {
    this.closeModalClicked.emit();
  }

  emitOutsideModalClicked() {
    this.outsideModalClicked.emit();
  }

  onTabClicked(tab: string) {
    this.#activeTab.set(tab);
  }

  submit() {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
    this.#formSubmitted.set(true);

    if (this.activeTab() === 'Login') {
      this.login();
    } else {
      this.register();
    }
  }

  login() {
    this.authService
      .login(this.form.value as UserLoginDto)
      .pipe(takeUntilDestroyed(this.#destroyRef))
      .subscribe({
        next: () => {
          this.enableForm();
        },
        error: (error) => {
          this.showErrorAndEnableForm(error);
        },
      });
  }

  register() {
    this.authService
      .register(this.form.value as UserRegisterDto)
      .pipe(takeUntilDestroyed(this.#destroyRef))
      .subscribe({
        next: () => {
          this.enableForm();
        },
        error: (error) => {
          this.showErrorAndEnableForm(error);
        },
      });
  }

  showErrorAndEnableForm(error: any) {
    this.form.enable();
    this.#formSubmitted.set(false);
    this.toastrService.error(
      error?.error?.detail || 'Something went wrong',
      'Error'
    );
  }

  enableForm() {
    this.emitCloseModalClicked();
    this.form.reset();
    this.form.enable();
    this.#formSubmitted.set(false);
  }
}

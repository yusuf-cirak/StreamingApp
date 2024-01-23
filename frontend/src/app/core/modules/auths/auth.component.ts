import {
  Component,
  EventEmitter,
  Input,
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
import { UserLoginDto } from '../../components/dtos/user-login-dto';
import { InputComponent } from '../../../shared/components/input/input.component';
import { RippleModule } from 'primeng/ripple';
import { LoadingIcon } from '../../../shared/icons/loading-icon';
import { ModalComponent } from '../../../shared/components/modal/modal.component';

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
  ],
  templateUrl: './auth.component.html',
})
export class AuthComponent {
  visible = input.required<boolean>({ alias: 'visible' });

  @Output() closeModalClicked = new EventEmitter<void>();

  readonly authService = inject(AuthService);

  readonly fb = inject(NonNullableFormBuilder);

  readonly form = this.fb.group({
    username: ['', [minLength(4, 'User name')]],
    password: ['', [minLength(4, 'Password')]],
  });

  emitCloseModalClicked() {
    this.closeModalClicked.emit();
  }

  login() {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
    // this.authService.login(this.form.value as UserLoginDto);
  }
}

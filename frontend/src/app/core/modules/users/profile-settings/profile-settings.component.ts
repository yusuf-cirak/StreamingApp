import { Component, inject, input, model, signal } from '@angular/core';
import { AuthService } from '../../auths/services/auth.service';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ImageService } from '../../../services/image.service';
import { TrashIcon } from '../../../../shared/icons/trash-icon';
import { LoadingIcon } from '../../../../shared/icons/loading-icon';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { UpdateProfileImageDto, UserService } from '../services/user-service';
import { catchError, finalize, tap } from 'rxjs';
import { CurrentUser } from '../../../models/current-user';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-profile-settings',
  standalone: true,
  templateUrl: './profile-settings.component.html',
  styleUrl: './profile-settings.component.scss',
  imports: [
    TrashIcon,
    LoadingIcon,
    ModalComponent,
    ReactiveFormsModule,
    InputComponent,
  ],
})
export class ProfileSettingsComponent {
  visible = model(false);

  readonly toastr = inject(ToastrService);
  readonly authService = inject(AuthService);
  readonly userService = inject(UserService);
  readonly user = this.authService.user;

  readonly imageService = inject(ImageService);

  readonly fb = inject(NonNullableFormBuilder);
  readonly form = this.fb.group({
    profileImage: this.imageService.getProfilePictureUrl(this.user()) as
      | string
      | File,
  });

  readonly formSubmitted = signal(false);

  readonly isImageUpdated = signal(false);
  isImageLoaded = signal(false);
  readonly imageUrl = signal<string>(this.form.value.profileImage as string);

  closeModal() {
    this.visible.set(false);
  }

  submit() {
    if (!this.form.dirty && this.form.touched) {
      this.toastr.info('You did not make any changes');
      return;
    }
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.formSubmitted.set(true);

    const profileImageUpdateDto: UpdateProfileImageDto = {
      profileImageUrl: this.isImageUpdated()
        ? ''
        : this.user()?.profileImageUrl!,
      profileImage: this.isImageUpdated()
        ? (this.form.value.profileImage as File)
        : (null as unknown as File),
    };

    this.userService
      .uploadProfileImage(profileImageUpdateDto)
      .pipe(
        tap((imageUrl) => {
          let user = this.user() as CurrentUser;
          user.profileImageUrl = imageUrl;
          this.authService.setUser(this.authService.currentUserToAuthDto(user));

          this.toastr.success('Profile image updated');
        }),
        catchError(() => {
          this.toastr.error('Profile image could not be updated');
          return [];
        }),
        finalize(() => {
          this.formSubmitted.set(false);
          this.isImageUpdated.set(false);
        })
      )
      .subscribe();
  }

  setFile(file?: File | string) {
    this.isImageUpdated.set(true);
    setTimeout(() => {
      this.form.patchValue({ profileImage: file });
      this.setUrlForProfileImage(file!);
    });
  }

  private setUrlForProfileImage(image: File | string) {
    if (!image) {
      this.setToDefaultProfileImage();
      return;
    }
    const reader = new FileReader();
    reader.onload = (e) => {
      this.imageUrl.set(e.target?.result as string);
    };
    reader.readAsDataURL(image as File);
  }

  setToDefaultProfileImage() {
    this.form.controls.profileImage.setValue(
      this.imageService.getDefaultProfilePictureUrl(this.user()?.username!)
    );

    this.imageUrl.set(this.form.value.profileImage as string);
  }
}

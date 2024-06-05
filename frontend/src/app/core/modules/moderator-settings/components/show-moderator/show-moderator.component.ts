import { Component, effect, inject, input, model, signal } from '@angular/core';
import {
  FormControl,
  FormsModule,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { GetStreamModeratorDto } from '../../contracts/get-stream-moderator-dto';
import { ModalComponent } from '../../../../../shared/components/modal/modal.component';
import { LoadingIcon } from '../../../../../shared/icons/loading-icon';
import { GetRoleDto } from '../../../permissions/contracts/get-role-dto';
import { InputComponent } from '../../../../../shared/components/input/input.component';
import { ChipListComponent } from '../../../../../shared/components/chip-list/chip-list.component';
import { LookupItem } from '../../../../../shared/api/lookup-item';
import { StreamerAvatarComponent } from '../../../streamers/components/streamer-avatar/streamer-avatar.component';
import { User } from '../../../../models';
import { ModeratorSearchService } from './services/moderator-search.service';
import { ModeratorSettingsService } from '../../services/moderator-settings.service';
import { ToastrService } from 'ngx-toastr';
import { catchError, tap } from 'rxjs';
@Component({
  selector: 'app-show-moderator',
  standalone: true,
  imports: [
    ModalComponent,
    ReactiveFormsModule,
    LoadingIcon,
    FormsModule,
    InputComponent,
    ChipListComponent,
    StreamerAvatarComponent,
  ],
  templateUrl: './show-moderator.component.html',
})
export class ShowModeratorComponent {
  visible = model(false);

  roles = input<GetRoleDto[]>();
  operationClaims = input<GetRoleDto[]>();

  search = signal('');
  userSuggestions = signal<User[]>([]);

  formSubmitted = signal(false);

  user = input<GetStreamModeratorDto>();

  private readonly toastr = inject(ToastrService);
  private readonly fb = inject(NonNullableFormBuilder);

  readonly moderatorSearchService = inject(ModeratorSearchService);

  private readonly moderatorService = inject(ModeratorSettingsService);

  readonly form = this.fb.group({
    roleIds: [[] as string[]],
    operationClaimIds: [[] as string[]],
  });

  ngOnInit() {
    this.initializeForm();
  }

  submit() {
    const roleIds = this.form.value.roleIds ?? [];
    const operationClaimIds = this.form.value.operationClaimIds ?? [];

    if (!roleIds.length && !operationClaimIds.length) {
      this.toastr.info('Please select at least one role or permission.');
      return;
    }

    this.formSubmitted.set(true);

    const userIds = this.chipUsers().map(
      (chipUser) => chipUser.value
    ) as string[];

    const upsertModerators = {
      userIds,
      roleIds,
      operationClaimIds,
    };

    this.moderatorService
      .upsertModerators(upsertModerators)
      .pipe(
        catchError((err) => {
          this.toastr.error(err.error.message);
          return [];
        }),
        tap(() => {
          this.moderatorService.update$.next();
          this.toastr.success('Operation completed successfully');
          this.visible.set(false);

          this.chipUsers.set([]);
          this.form.reset();
          this.formSubmitted.set(false);
        })
      )
      .subscribe();
  }

  chipUsers = signal<LookupItem[]>([]);

  removeFromChip(index: number) {
    this.chipUsers.update((chipUser) => chipUser.filter((_, i) => i !== index));
  }

  addToChip(item: User) {
    this.chipUsers.update((chipUsers) => [
      ...chipUsers,
      { key: item.username, value: item.id },
    ]);

    this.moderatorSearchService.clearTerm();
  }

  togglePermission(id: string, control: FormControl<string[]>) {
    const controlValue = control.value;
    const index = controlValue.indexOf(id);

    if (index === -1) {
      controlValue.push(id);
    } else {
      controlValue.splice(index, 1);
    }

    control.setValue(controlValue);
  }

  private initializeForm() {
    const user = this.user();
    if (!user) {
      return;
    }
    this.form.patchValue({
      roleIds: user.roleIds,
      operationClaimIds: user.operationClaimIds,
    });
    this.chipUsers.set([{ key: user.username, value: user.id }]);
  }
}

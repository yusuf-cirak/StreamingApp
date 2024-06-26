import { DatePipe, NgClass } from '@angular/common';
import {
  Component,
  computed,
  effect,
  inject,
  Signal,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { GetStreamBlockedUserDto } from './contracts/get-stream-blocked-user-dto';
import { CommunitySettingsService } from './services/community-settings.service';
import { ActivatedRoute } from '@angular/router';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import {
  catchError,
  EMPTY,
  map,
  merge,
  skip,
  startWith,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { StreamerAvatarComponent } from '../streamers/components/streamer-avatar/streamer-avatar.component';
import { CommunitySettingsSkeletonComponent } from './community-settings-skeleton/community-settings-skeleton.component';
import { DialogComponent } from '../../../shared/components/dialog/dialog.component';
import { ToastrService } from 'ngx-toastr';
import { CurrentCreatorService } from '../../layouts/creator/services/current-creator-service';
import { CreatorAuthService } from '../../layouts/creator/services/creator-auth-service';
import { UserAuthorizationService } from '../../services/user-authorization.service';
import { Roles } from '../../constants/roles';
import { OperationClaims } from '../../constants/operation-claims';
import { AuthService } from '../../services';
@Component({
  selector: 'app-community-settings',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    PaginatorModule,
    StreamerAvatarComponent,
    DatePipe,
    CommunitySettingsSkeletonComponent,
    DialogComponent,
  ],
  templateUrl: './community-settings.component.html',
})
export class CommunitySettingsComponent {
  private readonly toastr = inject(ToastrService);

  private readonly communitySettingsService = inject(CommunitySettingsService);

  readonly creatorService = inject(CurrentCreatorService);

  readonly authService = inject(AuthService);

  readonly streamerId = this.creatorService.streamerId as Signal<string>;

  readonly creatorAuthService = inject(CreatorAuthService);

  readonly userAuthorizationService = inject(UserAuthorizationService);

  readonly isAuthorized = signal(false);

  readonly update$ = new Subject<void>();

  #isLoaded = signal(false);
  readonly isLoaded = this.#isLoaded.asReadonly();

  #startIndex = signal(0);
  readonly startIndex = this.#startIndex.asReadonly();

  #rowCount = signal(5);
  readonly rowCount = this.#rowCount.asReadonly();

  filterKeyword = signal('');

  rows = toSignal(this.getStreamBlockedUsers());

  filterCols: Array<keyof GetStreamBlockedUserDto> = ['username', 'blockedAt'];

  filteredRows = computed(() => {
    const filter = this.filterKeyword();

    return (
      !filter.length
        ? this.rows()
        : this.rows()?.filter((row) =>
            this.filterCols.some((col) => row[col].includes(filter))
          )
    )?.slice(this.startIndex(), this.startIndex() + this.rowCount());
  });

  rowsLength = computed(() =>
    this.filterKeyword() ? this.filteredRows()?.length : this.rows()?.length
  );

  selectedRows = signal<GetStreamBlockedUserDto[] | undefined>(undefined);

  dialogVisible = signal(false);

  constructor() {
    effect(
      () => {
        const isAuthorized = this.userAuthorizationService.check({
          roles: [
            { name: Roles.StreamSuperModerator, value: this.streamerId() },
          ],
          operationClaims: [
            {
              name: OperationClaims.Stream.Write.BlockFromChat,
              value: this.streamerId(),
            },
          ],
          flags: [this.authService.userId() === this.streamerId()!],
        });

        this.isAuthorized.set(isAuthorized);
      },
      { allowSignalWrites: true }
    );
  }

  onPageChange(event: PaginatorState) {
    this.#startIndex.set(event.first!);
    this.#rowCount.set(event.rows!);
  }

  toDate(date: string) {
    return new Date(date);
  }

  selectRow(index: number, checked: boolean) {
    const row = this.filteredRows()![index];

    if (checked) {
      this.selectedRows.set([...(this.selectedRows() ?? []), row]);
    } else {
      this.selectedRows.set(
        (this.selectedRows() ?? []).filter((selectedRow) => selectedRow !== row)
      );
    }
  }

  selectAll(checked: boolean) {
    this.selectedRows.set(checked ? this.filteredRows() : []);
  }

  isSelected(row: GetStreamBlockedUserDto) {
    return this.selectedRows()?.some(
      (selectedRow) => selectedRow.id === row.id
    );
  }

  isAllSelected = computed(
    () => this.filteredRows()?.length === this.selectedRows()?.length
  );

  private getStreamBlockedUsers() {
    return merge(toObservable(this.streamerId), this.update$).pipe(
      switchMap(() => {
        return this.communitySettingsService.getBlockedUsers(this.streamerId());
      }),
      tap(() => {
        this.selectAll(false);
        this.#isLoaded.set(true);
      })
    );
  }

  onUnblockConfirm = () => {
    if (!this.selectedRows()?.length) {
      return;
    }
    this.dialogVisible.set(false);
    this.communitySettingsService
      .unblockUsers(
        this.streamerId()!,
        this.selectedRows()?.map((row) => row.id)!
      )
      .pipe(
        catchError(() => {
          this.toastr.error('Failed to unblock selected users');
          return EMPTY;
        }),
        tap(() => {
          this.toastr.success(
            'Selected users unblocked successfully',
            'Success'
          );
          this.update$.next();
        })
      )
      .subscribe();
  };
}

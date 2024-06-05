import { DatePipe, NgClass } from '@angular/common';
import { Component, computed, inject, Signal, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { catchError, EMPTY, merge, Subject, switchMap, tap } from 'rxjs';
import { StreamerAvatarComponent } from '../streamers/components/streamer-avatar/streamer-avatar.component';
import { DialogComponent } from '../../../shared/components/dialog/dialog.component';
import { ToastrService } from 'ngx-toastr';
import { CurrentCreatorService } from '../../layouts/creator/services/current-creator-service';
import { ModeratorSettingsSkeletonComponent } from './moderator-settings-skeleton/moderator-settings-skeleton.component';
import { GetStreamModeratorDto } from './contracts/get-stream-moderator-dto';
import { TrashIcon } from '../../../shared/icons/trash-icon';
import { PencilIcon } from '../../../shared/icons/pencil-icon';
import { PlusIcon } from '../../../shared/icons/plus-icon';
import { PermissionService } from '../permissions/services/permission.service';
import { ChipsModule } from 'primeng/chips';
import { ModeratorSettingsService } from './services/moderator-settings.service';
import { ShowModeratorComponent } from './components/show-moderator/show-moderator.component';
@Component({
  selector: 'app-moderator-settings',
  standalone: true,
  imports: [
    NgClass,
    FormsModule,
    PaginatorModule,
    StreamerAvatarComponent,
    DatePipe,
    ModeratorSettingsSkeletonComponent,
    DialogComponent,
    TrashIcon,
    PencilIcon,
    PlusIcon,
    ChipsModule,
    ShowModeratorComponent,
  ],
  providers: [ModeratorSettingsService],
  templateUrl: './moderator-settings.component.html',
})
export class ModeratorSettingsComponent {
  private readonly toastr = inject(ToastrService);

  readonly creatorService = inject(CurrentCreatorService);
  readonly permissionService = inject(PermissionService);

  readonly moderatorSettingsService = inject(ModeratorSettingsService);

  readonly streamerId = this.creatorService.streamerId as Signal<string>;

  #isLoaded = signal(false);
  readonly isLoaded = this.#isLoaded.asReadonly();

  #startIndex = signal(0);
  readonly startIndex = this.#startIndex.asReadonly();

  #rowCount = signal(5);
  readonly rowCount = this.#rowCount.asReadonly();

  filterKeyword = signal('');

  rows = toSignal(this.getStreamModerators());

  filterCols: Array<keyof GetStreamModeratorDto> = ['username'];

  filteredRows = computed(() => {
    const filter = this.filterKeyword();

    return (
      (!filter.length
        ? this.rows()
        : this.rows()?.filter((row) =>
            this.filterCols.some((col) => row[col].includes(filter))
          )
      )?.slice(this.startIndex(), this.startIndex() + this.rowCount()) ?? []
    );
  });

  rowsLength = computed(() =>
    this.filterKeyword() ? this.filteredRows()?.length : this.rows()?.length
  );

  selectedRows = signal<GetStreamModeratorDto[]>([]);

  firstSelectedRow = computed(() => this.selectedRows()?.[0]);

  dialogVisible = signal(false);
  showModeratorVisible = signal(false);

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

  isSelected(row: GetStreamModeratorDto) {
    return this.selectedRows()?.some(
      (selectedRow) => selectedRow.id === row.id
    );
  }

  isAllSelected = computed(
    () => this.filteredRows()?.length === this.selectedRows()?.length
  );

  private getStreamModerators() {
    return merge(
      toObservable(this.streamerId),
      this.moderatorSettingsService.update$
    ).pipe(
      switchMap(() => {
        return this.moderatorSettingsService.getModerators();
      }),
      tap(() => {
        this.selectAll(false);
        this.#isLoaded.set(true);
      })
    );
  }

  onRemoveConfirm = () => {
    if (!this.selectedRows()?.length) {
      return;
    }
    this.dialogVisible.set(false);
    this.moderatorSettingsService
      .removeModerators(this.selectedRows()?.map((row) => row.id)!)
      .pipe(
        catchError(() => {
          this.toastr.error('Failed to remove selected moderators');
          return EMPTY;
        }),
        tap(() => {
          this.toastr.success(
            'Selected moderators removed successfully',
            'Success'
          );
          this.moderatorSettingsService.update$.next();
        })
      )
      .subscribe();
  };
}

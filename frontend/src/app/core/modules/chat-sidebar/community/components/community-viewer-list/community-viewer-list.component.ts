import { Component, inject, input, signal } from '@angular/core';
import { User } from '@streaming-app/core';
import { CommunityViewerComponent } from '../community-viewer/community-viewer.component';
import { CommunityBlockService } from '../../services/community-block.service';
import { StreamBlockUserDto } from '../../../models/stream-block-user-dto';
import { ToastrService } from 'ngx-toastr';
import { DialogComponent } from '../../../../../../shared/components/dialog/dialog.component';
import { catchError, EMPTY, tap } from 'rxjs';

@Component({
  selector: 'app-community-viewer-list',
  standalone: true,
  imports: [CommunityViewerComponent, DialogComponent],
  providers: [CommunityBlockService],
  templateUrl: './community-viewer-list.component.html',
})
export class CommunityViewerListComponent {
  viewers = input.required<User[]>();
  private readonly communityBlockService = inject(CommunityBlockService);

  readonly selectedViewer = signal<StreamBlockUserDto | undefined>(undefined);
  readonly toastrService = inject(ToastrService);
  readonly dialogVisible = signal(false);

  openBlockViewerDialog(blockedUserDto: StreamBlockUserDto) {
    this.selectedViewer.set(blockedUserDto);
    this.dialogVisible.set(true);
  }

  // TODO: Do this in community-component
  onBlockViewerConfirm = () => {
    this.communityBlockService
      .block(this.selectedViewer()!)
      .pipe(
        tap(() => this.toastrService.success('User blocked successfully')),
        catchError(() => {
          this.toastrService.error('Failed to block user');
          return EMPTY;
        })
      )
      .subscribe();
  };
}

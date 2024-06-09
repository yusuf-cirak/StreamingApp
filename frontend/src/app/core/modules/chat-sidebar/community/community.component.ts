import { Component, effect, inject, signal } from '@angular/core';
import { CommunitySearchComponent } from './components/community-search/community-search.component';
import { CommunityProxyService } from './services/community-proxy.service';
import { StreamFacade } from '../../streams/services/stream.facade';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommunityViewService } from './services/community-view.service';
import { interval, of, startWith, switchMap } from 'rxjs';
import { CommunityViewerListComponent } from './components/community-viewer-list/community-viewer-list.component';
import { CommunityBlockService } from './services/community-block.service';
import { ModalComponent } from '../../../../shared/components/modal/modal.component';

@Component({
  selector: 'app-community',
  standalone: true,
  imports: [
    CommunitySearchComponent,
    CommunityViewerListComponent,
    ModalComponent,
  ],
  providers: [
    CommunityProxyService,
    CommunityViewService,
    CommunityBlockService,
  ],
  templateUrl: './community.component.html',
})
export class CommunityComponent {
  readonly streamFacade = inject(StreamFacade);
  readonly communityViewService = inject(CommunityViewService);
  readonly communityBlockService = inject(CommunityBlockService);

  readonly searchText = signal('');

  readonly viewers = toSignal(this.getCurrentViewers());

  readonly filteredViewers = signal(this.viewers());

  constructor() {
    this.registerSearchFilterEffect();
  }

  registerSearchFilterEffect() {
    effect(
      () => {
        const search = this.searchText();
        const viewers = this.viewers() || [];
        if (!search) {
          this.filteredViewers.set(viewers);
        } else {
          this.filteredViewers.set(
            viewers.filter((viewer) =>
              viewer.username.toLowerCase().includes(search.toLowerCase())
            )
          );
        }
      },
      { allowSignalWrites: true }
    );
  }

  getCurrentViewers() {
    return interval(20000).pipe(
      startWith(0),
      switchMap(() =>
        this.communityViewService.getCurrentViewers(
          this.streamFacade.streamerName()
        )
      )
    );
  }
}

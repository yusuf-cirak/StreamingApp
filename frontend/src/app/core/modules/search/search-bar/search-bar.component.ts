import { Component, ElementRef, ViewChild, inject } from '@angular/core';
import { InputComponent } from '../../../../shared/components/input/input.component';
import { SearchBarService } from '../services/search-bar.service';
import { StreamerAvatarComponent } from '../../streamers/components/streamer-avatar/streamer-avatar.component';
import { StreamerService } from '../../streamers/services/streamer.service';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  templateUrl: './search-bar.component.html',
  imports: [InputComponent, StreamerAvatarComponent],
})
export class SearchComponent {
  @ViewChild('searchInput') searchInput!: ElementRef;

  readonly searchBarService = inject(SearchBarService);

  readonly streamerService = inject(StreamerService);

  readonly term = this.searchBarService.term;
  readonly dropdownVisible = this.searchBarService.dropdownVisible;

  readonly searchResults = this.searchBarService.searchResults;

  setSearch(value: string) {
    this.searchBarService.setTerm(value);
    this.searchInput.nativeElement.focus();
  }

  clearSearch() {
    this.searchBarService.setDropdownVisible(false);
    this.searchBarService.setTerm('');
  }
}

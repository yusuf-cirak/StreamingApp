import { Component, ElementRef, model, viewChild } from '@angular/core';

@Component({
  selector: 'app-community-search',
  standalone: true,
  imports: [],
  templateUrl: './community-search.component.html',
})
export class CommunitySearchComponent {
  searchInput = viewChild<ElementRef>('searchInput');

  search = model('');

  setSearch(value: string) {
    this.search.set(value);
    this.searchInput()?.nativeElement.focus();
  }
}

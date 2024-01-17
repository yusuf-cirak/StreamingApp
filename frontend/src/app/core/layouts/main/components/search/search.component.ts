import {
  Component,
  ElementRef,
  ViewChild,
  inject,
  signal,
} from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  standalone: true,
  templateUrl: './search.component.html',
  imports: [],
})
export class SearchComponent {
  @ViewChild('searchInput') searchInput!: ElementRef;

  readonly #router = inject(Router);
  #search = signal<string>('');

  readonly search = this.#search.asReadonly();

  setSearch(value: string) {
    this.#search.set(value);
    this.searchInput.nativeElement.focus();
    this.setQueryParameter(value);
  }

  private setQueryParameter(value: string) {
    const queryParams = value ? { q: value } : {};
    this.#router.navigate([], { queryParams });
  }
}

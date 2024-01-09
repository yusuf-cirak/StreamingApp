import { Component, inject, signal } from '@angular/core';
import { HlmInputDirective } from '../../../../../../../libs/spartan/ui-input-helm/src/lib/hlm-input.directive';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [HlmInputDirective],
  templateUrl: './search.component.html',
})
export class SearchComponent {
  readonly #router = inject(Router);
  #search = signal<string>('');

  readonly search = this.#search.asReadonly();

  setSearch(value: string) {
    this.#search.set(value);

    this.setQueryParameter(value);
  }

  clearSearch() {
    this.#search.set('');
  }

  setQueryParameter(value: string) {
    const queryParams = value ? { q: value } : {};
    this.#router.navigate([], { queryParams });
  }
}

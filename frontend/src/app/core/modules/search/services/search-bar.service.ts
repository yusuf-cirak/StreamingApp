import { inject, Injectable, signal } from '@angular/core';
import { SearchProxyService } from './search-proxy.service';
import { debounceTime, Observable, of, switchMap, tap } from 'rxjs';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { StreamDto } from '../../streams/contracts/stream-dto';

@Injectable({
  providedIn: 'root',
})
export class SearchBarService {
  readonly searchProxyService = inject(SearchProxyService);
  readonly #router = inject(Router);

  readonly term = signal<string>('');

  readonly term$ = toObservable(this.term);

  readonly searchResults = toSignal(this.getSearchResults());

  readonly #dropdownVisible = signal(false);
  readonly dropdownVisible = this.#dropdownVisible.asReadonly();

  setTerm(value: string) {
    this.term.set(value);
  }

  setDropdownVisible(value: boolean) {
    this.#dropdownVisible.set(value);
  }

  getSearchResults() {
    return this.term$
      .pipe(
        debounceTime(300),
        switchMap((term) => {
          if (!term) {
            return of([]);
          }
          return this.searchProxyService.get(term);
        })
      )
      .pipe(tap(() => this.#dropdownVisible.set(true)));
  }

  onStreamerClick(streamer: StreamDto) {
    this.#router.navigate(['', streamer.user.username]);
    this.#dropdownVisible.set(false);
    this.term.set('');
  }
}

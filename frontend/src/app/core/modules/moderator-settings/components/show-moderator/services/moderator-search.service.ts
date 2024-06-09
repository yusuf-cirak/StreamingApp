import { inject, Injectable, signal } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { debounceTime, switchMap, of, tap, filter } from 'rxjs';
import { ModeratorSearchProxyService } from './moderator-search-proxy.service';

@Injectable({
  providedIn: 'root',
})
export class ModeratorSearchService {
  readonly moderatorSearchProxyService = inject(ModeratorSearchProxyService);
  readonly term = signal<string>('');

  readonly term$ = toObservable(this.term);

  readonly searchResults = toSignal(this.getSearchResults());

  readonly visible = signal(false);

  clearTerm() {
    this.term.set('');
    this.visible.set(false);
  }

  private getSearchResults() {
    return this.term$
      .pipe(
        debounceTime(300),
        filter((term) => term.length >= 2),
        switchMap((term) => {
          if (!term) {
            return of([]);
          }
          return this.moderatorSearchProxyService.get(term);
        })
      )
      .pipe(tap(() => this.visible.set(true)));
  }
}

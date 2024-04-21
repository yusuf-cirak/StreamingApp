import { Injectable } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageEventService {
  private readonly userChanged = new Subject<string | null>();
  readonly userChanged$ = this.userChanged.asObservable();

  constructor() {
    this.register();
  }

  register() {
    window.addEventListener('storage', (event) => {
      if (event.key === 'user' && event.storageArea === localStorage) {
        console.log('user changed from another window');
        this.userChanged.next(event.newValue);
      }
    });
  }
}

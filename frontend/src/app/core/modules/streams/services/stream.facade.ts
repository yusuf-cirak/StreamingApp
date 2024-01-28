import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class StreamFacade {
  #stream = signal<any>(undefined);

  readonly stream = this.#stream.asReadonly();

  setStream(stream: any) {
    this.#stream.set(stream);
  }
}

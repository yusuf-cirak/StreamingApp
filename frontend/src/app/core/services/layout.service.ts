import {
  HostListener,
  Injectable,
  computed,
  effect,
  inject,
  signal,
} from '@angular/core';
import { DOCUMENT } from '@angular/common';
@Injectable({
  providedIn: 'root',
})
export class LayoutService {
  readonly #sidebarOpen = signal(true);
  readonly sidebarOpen = computed(() => this.#sidebarOpen());

  readonly document = inject(DOCUMENT);

  constructor() {
    effect(() => {
      this.setSidebarClosed(this.#sidebarOpen());
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.#sidebarOpen.set(event.target.innerWidth > 640);
  }

  toggleSidebar() {
    const value = !this.sidebarOpen();
    this.#sidebarOpen.set(value);
  }

  setSidebarClosed(value: boolean) {
    const body = this.document.body;

    if (value) {
      body?.classList.remove('sidebar-closed');
    } else {
      body?.classList.add('sidebar-closed');
    }
  }
}

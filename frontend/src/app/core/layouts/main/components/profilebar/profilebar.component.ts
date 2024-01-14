import {
  Component,
  ElementRef,
  HostListener,
  ViewChild,
  computed,
  signal,
} from '@angular/core';
import { fadeAnimation } from '../../../../../shared/animations/fade-animation';

@Component({
  selector: 'app-main-profilebar',
  standalone: true,
  imports: [],
  templateUrl: './profilebar.component.html',
  animations: [fadeAnimation],
})
export class ProfilebarComponent {
  #profileMenuOpen = signal(false);

  readonly profileMenuOpen = this.#profileMenuOpen.asReadonly();

  readonly isProfileMenuOpen = computed(() => this.profileMenuOpen());

  closeProfileMenu() {
    this.#profileMenuOpen.set(false);
  }

  toggleProfileMenu() {
    const value = this.profileMenuOpen();
    this.#profileMenuOpen.set(!value);
  }

  @ViewChild('profileMenuWrapperRef') profileMenuWrapperRef!: ElementRef;

  @HostListener('document:click', ['$event'])
  click(event: Event) {
    if (!this.profileMenuWrapperRef?.nativeElement?.contains(event.target)) {
      this.closeProfileMenu();
    }
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: Event) {
    if (!this.profileMenuWrapperRef?.nativeElement?.contains(event.target)) {
      this.closeProfileMenu();
    }
  }
  logout() {}
}

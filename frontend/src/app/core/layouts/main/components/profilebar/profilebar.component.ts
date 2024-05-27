import {
  Component,
  ElementRef,
  HostListener,
  ViewChild,
  computed,
  inject,
  signal,
} from '@angular/core';
import { fadeAnimation } from '../../../../../shared/animations/fade-animation';
import { AuthService } from '@streaming-app/core';
import { ImageService } from '../../../../services/image.service';
import { StreamerAvatarComponent } from '../../../../modules/streamers/components/streamer-avatar/streamer-avatar.component';

@Component({
  selector: 'app-main-profilebar',
  standalone: true,
  imports: [StreamerAvatarComponent],
  templateUrl: './profilebar.component.html',
  animations: [fadeAnimation],
})
export class ProfilebarComponent {
  readonly authService = inject(AuthService);
  readonly imageService = inject(ImageService);
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
  logout() {
    this.authService.logout();
  }
}

import {
  Component,
  ElementRef,
  HostListener,
  ViewChild,
  inject,
  signal,
} from '@angular/core';
import { fadeAnimation } from '../../../../../shared/animations/fade-animation';
import { LayoutService } from '../../../../services/layout.service';
import { NonNullableFormBuilder } from '@angular/forms';

@Component({
  selector: 'app-profilebar',
  standalone: true,
  imports: [],
  templateUrl: './profilebar.component.html',
  animations: [fadeAnimation],
})
export class ProfilebarComponent {
  readonly layoutService = inject(LayoutService);

  @ViewChild('profileMenuWrapperRef') profileMenuWrapperRef!: ElementRef;

  @HostListener('document:click', ['$event'])
  click(event: Event) {
    // if (this.layoutService.isMobile()) {
    //   if (
    //     !this.mobileAsideElementRef?.nativeElement?.contains(event.target) &&
    //     !this.mobileHamburgerMenuRef?.nativeElement?.contains(event.target)
    //   ) {
    //     this.layoutService.closeSideMenu();
    //   }
    // }

    if (!this.profileMenuWrapperRef?.nativeElement?.contains(event.target)) {
      this.layoutService.closeProfileMenu();
    }
  }

  @HostListener('document:keydown.escape', ['$event'])
  onEscape(event: Event) {
    // this.layoutService.closeSideMenu();

    if (!this.profileMenuWrapperRef?.nativeElement?.contains(event.target)) {
      this.layoutService.closeProfileMenu();
    }
  }
  logout() {}
}

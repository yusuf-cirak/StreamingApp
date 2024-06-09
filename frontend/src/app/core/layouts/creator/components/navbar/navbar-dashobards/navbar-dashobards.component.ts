import {
  Component,
  ElementRef,
  HostListener,
  inject,
  signal,
  viewChild,
} from '@angular/core';
import { User } from '../../../../../models';
import { popoverAnimation } from '../../../../../../shared/animations/popover-animation';
import { StreamerAvatarComponent } from '../../../../../modules/streamers/components/streamer-avatar/streamer-avatar.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CurrentCreatorService } from '../../../services/current-creator-service';
import { CreatorService } from '../../../services/creator-service';
import { AuthService } from '../../../../../services';
import { BehaviorSubject, filter, Observable, of, switchMap, tap } from 'rxjs';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-navbar-dashobards',
  standalone: true,
  imports: [StreamerAvatarComponent],
  animations: [popoverAnimation],
  templateUrl: './navbar-dashobards.component.html',
})
export class NavbarDashobardsComponent {
  readonly currentCreatorService = inject(CurrentCreatorService);

  readonly selectStreamer$ = new BehaviorSubject<Observable<User | undefined>>(
    toObservable(this.currentCreatorService.streamer)
  );

  readonly router = inject(Router);
  readonly activatedRoute = inject(ActivatedRoute);
  readonly creatorService = inject(CreatorService);
  readonly authService = inject(AuthService);
  visible = signal(false);

  readonly containerRef = viewChild<ElementRef>('containerRef');

  readonly moderatingStreams = this.creatorService.moderatingStreams;
  selectedStreamer = toSignal(this.getSelectedStreamer());

  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const containerRef = this.containerRef()?.nativeElement;

    if (containerRef && !containerRef.contains(target) && this.visible()) {
      this.visible.set(false);
    }
  }

  toggleVisible() {
    this.visible.set(!this.visible());
  }

  selectDashboard(streamer: User) {
    this.selectStreamer$.next(of(streamer));
  }

  private getSelectedStreamer() {
    return this.selectStreamer$.pipe(
      switchMap((source) => source),
      filter(Boolean),
      tap((user) => {
        const currentStreamerUsername = this.selectedStreamer()?.username;

        this.currentCreatorService.update(user);

        const currentUrl = this.router.url;

        this.router.navigateByUrl(
          currentUrl.replace(currentStreamerUsername!, user.username)
        );
      })
    );
  }
}

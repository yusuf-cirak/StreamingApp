import { Component, Input, signal } from '@angular/core';
import { OfflineStreamerIcon } from '@streaming-app/shared/icons';

@Component({
  selector: 'app-offline-stream',
  standalone: true,
  templateUrl: './offline-stream.component.html',
  imports: [OfflineStreamerIcon],
})
export class OfflineStreamComponent {
  streamerName = signal<string | undefined>(undefined);
  @Input('streamerName') set streamerNameSetter(name: string) {
    this.streamerName.set(name);
  }
}

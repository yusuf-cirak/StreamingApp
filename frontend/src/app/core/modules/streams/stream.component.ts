import { Component, signal } from '@angular/core';
import { LiveStreamComponent } from './components/live-stream/live-stream.component';
import { OfflineStreamComponent } from './components/offline-stream/offline-stream.component';

@Component({
  selector: 'app-stream',
  standalone: true,
  templateUrl: './stream.component.html',
  imports: [LiveStreamComponent, OfflineStreamComponent],
})
export class StreamComponent {
  #loaded = signal(false);
  readonly loaded = this.#loaded.asReadonly();
}

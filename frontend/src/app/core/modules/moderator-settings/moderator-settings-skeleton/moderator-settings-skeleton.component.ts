import { Component, model, signal } from '@angular/core';

@Component({
  selector: 'app-moderator-settings-skeleton',
  standalone: true,
  templateUrl: './moderator-settings-skeleton.component.html',
})
export class ModeratorSettingsSkeletonComponent {
  itemCount = model(5);

  items = signal(Array.from({ length: this.itemCount() }, (_, i) => i));
}

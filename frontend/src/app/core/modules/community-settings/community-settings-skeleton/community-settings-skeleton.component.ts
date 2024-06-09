import { Component, model, signal } from '@angular/core';

@Component({
  selector: 'app-community-settings-skeleton',
  standalone: true,
  templateUrl: './community-settings-skeleton.component.html',
})
export class CommunitySettingsSkeletonComponent {
  itemCount = model(5);

  items = signal(Array.from({ length: this.itemCount() }, (_, i) => i));
}

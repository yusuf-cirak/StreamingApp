import { Component, input, model, output } from '@angular/core';
import { LookupItem } from '../../api/lookup-item';

@Component({
  selector: 'app-chip-list',
  standalone: true,
  templateUrl: './chip-list.component.html',
})
export class ChipListComponent {
  items = model<LookupItem[]>();
  displayKey = input<keyof LookupItem>('key');

  onRemoveClick = output<number>();
}

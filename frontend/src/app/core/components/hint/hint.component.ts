import { Component, input } from '@angular/core';
import { InfoIcon } from '../../../shared/icons/info-icon';

@Component({
  selector: 'app-hint',
  standalone: true,
  imports: [InfoIcon],
  templateUrl: './hint.component.html',
})
export class HintComponent {
  text = input.required<string>();
}

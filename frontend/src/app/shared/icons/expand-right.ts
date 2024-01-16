import { Component } from '@angular/core';
import { HlmTooltipComponent } from '../../../../libs/spartan/ui-tooltip-helm/src/lib/hlm-tooltip.component';
import { HlmTooltipTriggerDirective } from '../../../../libs/spartan/ui-tooltip-helm/src/lib/hlm-tooltip-trigger.directive';
import { BrnTooltipContentDirective } from '@spartan-ng/ui-tooltip-brain';

@Component({
  selector: 'app-expand-right-icon',
  standalone: true,
  imports: [
    HlmTooltipComponent,
    HlmTooltipTriggerDirective,
    BrnTooltipContentDirective,
  ],
  template: `
    <hlm-tooltip>
      <svg
        hlmTooltipTrigger
        aria-describedby="expand-right-tooltip"
        fill="currentColor"
        class="w-8 h-8"
        viewBox="0 0 24 24"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M15.2928932,12 L12.1464466,8.85355339 C11.9511845,8.65829124 11.9511845,8.34170876 12.1464466,8.14644661 C12.3417088,7.95118446 12.6582912,7.95118446 12.8535534,8.14644661 L16.8535534,12.1464466 C17.0488155,12.3417088 17.0488155,12.6582912 16.8535534,12.8535534 L12.8535534,16.8535534 C12.6582912,17.0488155 12.3417088,17.0488155 12.1464466,16.8535534 C11.9511845,16.6582912 11.9511845,16.3417088 12.1464466,16.1464466 L15.2928932,13 L4.5,13 C4.22385763,13 4,12.7761424 4,12.5 C4,12.2238576 4.22385763,12 4.5,12 L15.2928932,12 Z M19,5.5 C19,5.22385763 19.2238576,5 19.5,5 C19.7761424,5 20,5.22385763 20,5.5 L20,19.5 C20,19.7761424 19.7761424,20 19.5,20 C19.2238576,20 19,19.7761424 19,19.5 L19,5.5 Z"
        />
      </svg>
      <strong *brnTooltipContent>Expand</strong>
    </hlm-tooltip>
  `,
})
export class ExpandRightIcon {}

import { Component } from '@angular/core';
import { HlmTooltipComponent } from '../../../../libs/spartan/ui-tooltip-helm/src/lib/hlm-tooltip.component';
import { HlmTooltipTriggerDirective } from '../../../../libs/spartan/ui-tooltip-helm/src/lib/hlm-tooltip-trigger.directive';
import { BrnTooltipContentDirective } from '@spartan-ng/ui-tooltip-brain';

@Component({
  selector: 'app-collapse-left-icon',
  standalone: true,
  imports: [
    HlmTooltipComponent,
    HlmTooltipTriggerDirective,
    BrnTooltipContentDirective,
  ],
  template: `
    <hlm-tooltip>
      <svg
        aria-describedby="collapse-tooltip"
        hlmTooltipTrigger
        fill="currentColor"
        class="w-6 h-6"
        viewBox="0 0 24 24"
        xmlns="http://www.w3.org/2000/svg"
      >
        <path
          d="M8.70710678,12 L19.5,12 C19.7761424,12 20,12.2238576 20,12.5 C20,12.7761424 19.7761424,13 19.5,13 L8.70710678,13 L11.8535534,16.1464466 C12.0488155,16.3417088 12.0488155,16.6582912 11.8535534,16.8535534 C11.6582912,17.0488155 11.3417088,17.0488155 11.1464466,16.8535534 L7.14644661,12.8535534 C6.95118446,12.6582912 6.95118446,12.3417088 7.14644661,12.1464466 L11.1464466,8.14644661 C11.3417088,7.95118446 11.6582912,7.95118446 11.8535534,8.14644661 C12.0488155,8.34170876 12.0488155,8.65829124 11.8535534,8.85355339 L8.70710678,12 L8.70710678,12 Z M4,5.5 C4,5.22385763 4.22385763,5 4.5,5 C4.77614237,5 5,5.22385763 5,5.5 L5,19.5 C5,19.7761424 4.77614237,20 4.5,20 C4.22385763,20 4,19.7761424 4,19.5 L4,5.5 Z"
        />
      </svg>
      <span *brnTooltipContent>Collapse</span>
    </hlm-tooltip>
  `,
})
export class CollapseLeftIcon {}

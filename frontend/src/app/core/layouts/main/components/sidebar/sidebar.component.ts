import { NgClass } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CollapseLeftIcon } from '../../../../../shared/icons/collapse-left';
import { ExpandRightIcon } from '../../../../../shared/icons/expand-right';
import { expandCollapseAnimation } from '../../../../../shared/animations/expand-collapse-animation';
import { LayoutService } from '../../../../services/layout.service';
import { RecommendedStreamersComponent } from '../../../../modules/recommended-streamers/recommended-streamers.component';

@Component({
  selector: 'app-main-sidebar',
  standalone: true,
  imports: [
    NgClass,
    CollapseLeftIcon,
    ExpandRightIcon,
    RecommendedStreamersComponent,
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  animations: [expandCollapseAnimation],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent {
  readonly layoutService = inject(LayoutService);
}

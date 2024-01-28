import { Component, EventEmitter, Output, input } from '@angular/core';

@Component({
  selector: 'app-tab-panel',
  standalone: true,
  templateUrl: './tab-panel.component.html',
})
export class TabPanelComponent {
  tabs = input.required<string[]>();

  activeTab = input.required<string>();

  @Output() tabClicked = new EventEmitter<string>();

  emitTabClicked(tab: string) {
    this.tabClicked.emit(tab);
  }
}

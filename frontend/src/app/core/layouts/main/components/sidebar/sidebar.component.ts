import { NgClass } from '@angular/common';
import { Component, HostListener, inject } from '@angular/core';
import { CollapseLeftIcon } from '../../../../../shared/icons/collapse-left';
import { ExpandRightIcon } from '../../../../../shared/icons/expand-right';
import { expandCollapseAnimation } from '../../../../../shared/animations/expand-collapse-animation';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [NgClass, CollapseLeftIcon, ExpandRightIcon],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  animations: [expandCollapseAnimation],
})
export class SidebarComponent {
  isList: number = 0;
  isMenu: boolean = false;
  isMenuBtn() {
    this.isMenu = !this.isMenu;
  }
  isSearch: boolean = false;
  constructor() {}
  ngOnInit(): void {}

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    if (event.target.innerWidth <= 640) {
      this.isMenu = true;
    } else {
      this.isMenu = false;
    }
  }
}

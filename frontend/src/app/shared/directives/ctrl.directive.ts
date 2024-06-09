import { Directive, HostListener, model, output } from '@angular/core';

@Directive({
  selector: '[appCtrl]',
  standalone: true,
})
export class CtrlDirective {
  ctrlPressed = model(false);

  @HostListener('document:keydown.control', ['$event'])
  onCtrlDown() {
    if (!this.ctrlPressed()) {
      this.ctrlPressed.set(true);
    }
  }
  @HostListener('document:keyup.control', ['$event'])
  onCtrlUp() {
    this.ctrlPressed.set(false);
  }
}

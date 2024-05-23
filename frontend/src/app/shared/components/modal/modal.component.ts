import { CommonModule, NgTemplateOutlet } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Output,
  TemplateRef,
  ViewChild,
  input,
  model,
} from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [NgTemplateOutlet],
  templateUrl: './modal.component.html',
})
export class ModalComponent {
  visible = model(false);
  header = input<string>('');

  headerTemplate = input<TemplateRef<ElementRef> | null>(null);

  bodyTemplate = input<TemplateRef<ElementRef> | null>(null);

  @ViewChild('modalRef') modalRef!: ElementRef;

  @Output() closeModalClicked = new EventEmitter<void>();
  @Output() outsideModalClicked = new EventEmitter<void>();

  emitCloseModalClicked() {
    this.closeModalClicked.emit();
  }

  emitOutsideClicked() {
    this.outsideModalClicked.emit();
  }

  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler() {
    this.emitOutsideClicked();
  }
}

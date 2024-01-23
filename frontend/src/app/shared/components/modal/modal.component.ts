import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  Output,
  TemplateRef,
  input,
} from '@angular/core';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss',
})
export class ModalComponent {
  modalHeaderTitle = input.required({ alias: 'modalHeaderTitle' });

  modalBodyTemplate = input<TemplateRef<ElementRef> | null>(null);

  @Output() closeModalClicked = new EventEmitter<void>();

  emitCloseModalClicked() {
    this.closeModalClicked.emit();
  }
}

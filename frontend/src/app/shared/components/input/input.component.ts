import { KeyValuePipe, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { LookupItem } from '../../api/lookup-item';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [NgIf, NgFor, ReactiveFormsModule, NgClass, KeyValuePipe],
  templateUrl: './input.component.html',
})
export class InputComponent {
  @Input() label?: string;

  @Input() autocomplete?: string;

  @Input() required?: boolean;

  @Input() disabled: boolean = false;

  @Input('disabled') set disabledSetter(value: boolean) {
    this.disabled = value;
    if (this.control) {
      if (value) {
        this.control.disable();
      } else {
        this.control.enable();
      }
    }
  }
  @Input() readonly?: boolean;

  @Input() placeholder?: string | undefined;

  @Input() inputType?: 'text' | 'number' | 'password';

  @Input() inputClass?: string = '';

  @Input() control?: FormControl;
}

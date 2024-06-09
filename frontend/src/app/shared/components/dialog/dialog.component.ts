import { Component, input, model, output } from '@angular/core';

@Component({
  selector: 'app-dialog',
  standalone: true,
  templateUrl: './dialog.component.html',
})
export class DialogComponent {
  headerText = input<string>('Confirmation');

  messageText = input<string>('Are you sure you want to proceed?');

  confirmText = input<string>('Confirm');

  cancelText = input<string>('Cancel');

  onConfirm = input<() => unknown>(() => {});

  onCancel = input<() => unknown>(() => {});

  visible = model(false);

  confirm = () => {
    this.visible.set(false);
    const confirm = this.onConfirm();
    confirm();
  };

  cancel = () => {
    this.visible.set(false);
    const cancel = this.onCancel();
    cancel();
  };
}

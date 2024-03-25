import {
  ChangeDetectorRef,
  Component,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import { Button, ButtonModule } from 'primeng/button';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  selector: 'app-copy-clipboard',
  standalone: true,
  imports: [ButtonModule],
  template: `<p-button
    #button
    [icon]="icon()"
    [text]="true"
    severity="secondary"
    (onClick)="copyToClipboard(button)"
  ></p-button> `,
})
export class CopyClipboardComponent {
  copyText = input.required<string>();

  readonly icon = signal<string>('pi pi-copy');

  copied = output<void>();

  readonly #clipboard = inject(Clipboard);

  copyToClipboard(button: Button) {
    this.#clipboard.copy(this.copyText());
    this.copied.emit();

    this.icon.update(() => 'pi pi-check');

    setInterval(() => {
      this.icon.update(() => 'pi pi-copy');
    }, 1000);
  }
}

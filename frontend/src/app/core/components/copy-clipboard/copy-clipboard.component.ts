import { Component, Input, inject, signal } from '@angular/core';
import { Button, ButtonModule } from 'primeng/button';
import { Clipboard } from '@angular/cdk/clipboard';

@Component({
  selector: 'app-copy-clipboard',
  standalone: true,
  imports: [ButtonModule],
  template: `<p-button
    #button
    icon="pi pi-copy"
    [text]="true"
    severity="secondary"
    (onClick)="copyToClipboard(button)"
  ></p-button> `,
})
export class CopyClipboardComponent {
  #copyText = signal<string>('');
  @Input('copyText') set copyTextSetter(value: string) {
    this.#copyText.set(value);
  }

  readonly #clipboard = inject(Clipboard);

  copyToClipboard(button: Button) {
    this.#clipboard.copy(this.#copyText());

    button.icon = 'pi pi-check';

    setInterval(() => {
      button.icon = 'pi pi-copy';
    }, 1000);
  }
}

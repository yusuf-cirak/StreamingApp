import { Directive, ElementRef, inject, OnInit } from '@angular/core';
import { textHashColor } from '../utils';

@Directive({
  selector: '[appTextHashColor]',
  standalone: true,
})
export class TextHashColorDirective implements OnInit {
  element = inject(ElementRef);

  ngOnInit() {
    this.changeTextColor();
  }

  changeTextColor() {
    const color = textHashColor(this.element.nativeElement.textContent);
    this.element.nativeElement.style.color = color;
  }
}

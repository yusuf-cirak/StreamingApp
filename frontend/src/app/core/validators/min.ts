import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const min =
  (min: number, message: string): ValidatorFn =>
  (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    return value < min
      ? {
          min: message,
        }
      : null;
  };

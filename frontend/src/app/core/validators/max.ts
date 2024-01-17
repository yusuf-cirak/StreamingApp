import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const max =
  (max: number, message: string): ValidatorFn =>
  (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    return value > max
      ? {
          max: message,
        }
      : null;
  };

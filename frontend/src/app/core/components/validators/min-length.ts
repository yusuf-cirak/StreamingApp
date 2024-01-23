import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const minLength =
  (min: number, controlName: string): ValidatorFn =>
  (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    return value.length < min
      ? {
          minLength: `${controlName} should be at least ${min} characters long`,
        }
      : null;
  };

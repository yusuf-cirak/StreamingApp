import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const maxLength =
  (max: number, controlName: string): ValidatorFn =>
  (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    return value.length > max
      ? {
          maxLength: `${controlName} should be at most ${max} characters long`,
        }
      : null;
  };

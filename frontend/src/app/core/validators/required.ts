import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

export const required =
  (controlName: string): ValidatorFn =>
  (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    return !value?.length
      ? {
          required: `${controlName} is required`,
        }
      : null;
  };

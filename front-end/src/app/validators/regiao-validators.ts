import { AbstractControl, AsyncValidatorFn, FormArray, ValidatorFn } from '@angular/forms';
import { of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { RegiaoService } from '../services/regiao.service';

export function atLeastOneCity(): ValidatorFn {
  return (control: AbstractControl) => {
    const arr = control as FormArray;
    return arr.length > 0 ? null : { atLeastOneCity: true };
  };
}

export function noDuplicateCities(): ValidatorFn {
  return (control: AbstractControl) => {
    const arr = (control as FormArray).value as (string | null)[];
    const ids = arr.filter(Boolean) as string[];
    return new Set(ids).size === ids.length ? null : { duplicateCities: true };
  };
}

export const nomeUnicoValidator = (regiaoSvc: RegiaoService, ignoreId?: string): AsyncValidatorFn => {
  return (control: AbstractControl) => {
    const nome = (control.value ?? '').trim();
    if (!nome) return of(null);
    return regiaoSvc.existsByName(nome, ignoreId).pipe(
      map(exists => (exists ? { nomeDuplicado: true } : null)),
      catchError(() => of(null))
    );
  };
};

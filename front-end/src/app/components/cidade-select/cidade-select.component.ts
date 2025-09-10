import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Cidade } from '../../models/cidade';
import { CidadeService } from '../../services/cidade.service';

@Component({
  selector: 'app-cidade-select',
  templateUrl: './cidade-select.component.html',
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => CidadeSelectComponent), multi: true }
  ]
})
export class CidadeSelectComponent implements ControlValueAccessor, OnInit {
  cidades$!: Observable<Cidade[]>;
  value: string | null = null;
  disabled = false;

  private onChange: (v: any) => void = () => {};
  private onTouched: () => void = () => {};

  constructor(private cidadeSvc: CidadeService) {}

  ngOnInit(): void {
    this.cidades$ = this.cidadeSvc.list();

    this.cidades$.pipe(take(1)).subscribe(() => {
      if (this.value) {
        this.onChange(this.value);
      }
    });
  }

  writeValue(v: any): void { this.value = v || null; }
  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouched = fn; }
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  onSelectChange(raw: string) {
    const val = raw || null;
    this.value = val;
    this.onChange(val);
    this.onTouched();
  }
}

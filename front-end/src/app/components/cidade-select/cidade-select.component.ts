import { Component, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { take } from 'rxjs/operators';
import { CidadeService } from '../../services/cidade.service';
import { Cidade } from '../../models/cidade';

@Component({
  selector: 'app-cidade-select',
  templateUrl: './cidade-select.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CidadeSelectComponent),
    multi: true,
  }],
})
export class CidadeSelectComponent implements ControlValueAccessor, OnInit {
  cidades: Cidade[] = [];
  value: string | null = null;
  disabled = false;

  private onChange: (v: string | null) => void = () => {};
  private onTouched: () => void = () => {};

  constructor(private cidadesSvc: CidadeService) {}

  ngOnInit(): void {
    this.cidadesSvc.list().pipe(take(1)).subscribe(list => {
      this.cidades = list;
    });
  }

  writeValue(v: string | null): void { this.value = v ?? null; }
  registerOnChange(fn: any): void { this.onChange = fn; }
  registerOnTouched(fn: any): void { this.onTouched = fn; }
  setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

  onModelChange(v: string | null) {
    this.value = v ?? null;
    this.onChange(this.value);
    this.onTouched();
  }
}

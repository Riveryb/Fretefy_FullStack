import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';

import { RegiaoService } from '../../../services/regiao.service';
import { atLeastOneCity, noDuplicateCities, nomeUnicoValidator } from '../../../validators/regiao-validators';
import { Regiao } from '../../../models/regiao';

@Component({
  selector: 'app-regiao-form',
  templateUrl: './regiao-form.component.html'
})
export class RegiaoFormComponent implements OnInit {
  form: FormGroup;
  editingId: string | null = null;
  loading = true;
  errorMsg = '';

  constructor(
    private fb: FormBuilder,
    private regiaoSvc: RegiaoService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  get cidades(): FormArray {
    return this.form.get('cidades') as FormArray;
  }

  ngOnInit(): void {
    this.editingId = this.route.snapshot.paramMap.get('id');

    this.form = this.fb.group({
      nome: ['', {
        validators: [Validators.required],
        asyncValidators: [nomeUnicoValidator(this.regiaoSvc, this.editingId || undefined)],
        updateOn: 'blur'
      }],
      cidades: this.fb.array([], [atLeastOneCity(), noDuplicateCities()])
    });

    if (this.editingId) {
      this.regiaoSvc.get(this.editingId).pipe(take(1)).subscribe(
        (r: Regiao) => {
          (this.form.get('nome') as FormControl).setValue(r.nome);

          while (this.cidades.length) this.cidades.removeAt(0);
          if (r.regiaoCidades && r.regiaoCidades.length) {
            r.regiaoCidades.forEach(rc =>
              this.cidades.push(new FormControl(rc.cidadeId, Validators.required))
            );
          } else {
            this.addCidade();
          }

          this.loading = false;
        },
        _ => { this.errorMsg = 'Falha ao carregar a região.'; this.loading = false; }
      );
    } else {
      this.addCidade();
      this.loading = false;
    }
  }

  addCidade(): void {
    this.cidades.push(new FormControl(null, Validators.required));
  }

  removeCidade(i: number): void {
    this.cidades.removeAt(i);
    this.cidades.updateValueAndValidity();
  }

  salvar(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const val: any = this.form.value;
    const dto = {
      nome: (val.nome || '').trim(),
      cidadeIds: (val.cidades as any[]).filter(Boolean)
    };

    const req$ = this.editingId
      ? this.regiaoSvc.update(this.editingId, dto)
      : this.regiaoSvc.create(dto);

    req$.pipe(take(1)).subscribe(
      () => this.router.navigate(['/regiao']),
      err => this.errorMsg = err?.error?.mensagem ?? 'Erro ao salvar.'
    );
  }

  cancelar(): void {
    this.router.navigate(['/regiao']);
  }
}

import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';

import { RegiaoService } from '../../../services/regiao.service';
import { atLeastOneCity, noDuplicateCities, nomeUnicoValidator } from '../../../validators/regiao-validators';

@Component({
  selector: 'app-regiao-form',
  templateUrl: './regiao-form.component.html'
})
export class RegiaoFormComponent implements OnInit {
  form!: FormGroup;
  editingId: string | null = null;
  loading = true;
  errorMsg = '';

  // SEM typed forms (pré-Angular 14)
  get cidades(): FormArray {
    return this.form.get('cidades') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private regiaoSvc: RegiaoService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.editingId = this.route.snapshot.paramMap.get('id');

    this.form = this.fb.group({
      nome: ['', {
        validators: [Validators.required],
        asyncValidators: [nomeUnicoValidator(this.regiaoSvc, this.editingId ?? undefined)],
        updateOn: 'blur'
      }],

      cidades: this.fb.array([], [atLeastOneCity(), noDuplicateCities()])
    });

    if (this.editingId) {
      this.regiaoSvc.get(this.editingId).pipe(take(1)).subscribe({
        next: (r) => {
          this.form.get('nome')?.setValue(r.nome);

          r.regiaoCidades.forEach(rc => {
            const ctrl = new FormControl(rc.cidadeId, Validators.required);
            this.cidades.push(ctrl);
          });

          if (this.cidades.length === 0) this.addCidade();
          this.loading = false;
        },
        error: () => { this.errorMsg = 'Falha ao carregar a região.'; this.loading = false; }
      });
    } else {
      this.addCidade();
      this.loading = false;
    }
  }

  addCidade() {
    this.cidades.push(new FormControl(null, Validators.required));
  }

  removeCidade(i: number) {
    this.cidades.removeAt(i);
    this.cidades.updateValueAndValidity();
  }

  salvar() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    const dto = {
      nome: (this.form.value.nome as string).trim(),
      cidadeIds: (this.form.value.cidades as (string|null)[]).filter(Boolean) as string[],
    };

    const req$ = this.editingId
      ? this.regiaoSvc.update(this.editingId, dto)
      : this.regiaoSvc.create(dto);

    req$.pipe(take(1)).subscribe({
      next: () => this.router.navigate(['/regiao']),
      error: (err) => this.errorMsg = err?.error?.mensagem ?? 'Erro ao salvar.'
    });
  }

  cancelar() { this.router.navigate(['/regiao']); }
}

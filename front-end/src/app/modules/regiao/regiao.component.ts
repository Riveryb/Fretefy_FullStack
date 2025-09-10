import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Regiao } from '../../models/regiao';
import { RegiaoService } from '../../services/regiao.service';

@Component({
  selector: 'app-regiao',
  templateUrl: './regiao.component.html'
})
export class RegiaoComponent implements OnInit {
  regioes$!: Observable<Regiao[]>;

  constructor(private regiaoSvc: RegiaoService) {}

  ngOnInit(): void { this.load(); }

  load() {
    // com cidades pra montar a string bonitinha
    this.regioes$ = this.regiaoSvc.listWithCidades();
  }

  ativar(r: Regiao)    { this.regiaoSvc.ativar(r.id).subscribe(() => this.load()); }
  desativar(r: Regiao) { this.regiaoSvc.desativar(r.id).subscribe(() => this.load()); }
}

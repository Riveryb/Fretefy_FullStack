import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Regiao } from '../models/regiao';

@Injectable({ providedIn: 'root' })
export class RegiaoService {
  private base = '/api/regiao';

  constructor(private http: HttpClient) {}

  private normalizeRegiao = (r: any): Regiao => {
    const toCidade = (c: any) => c ? ({
      id: c.id ?? c.Id,
      nome: c.nome ?? c.Nome,
      uf: c.uf ?? c.UF
    }) : undefined;

    const links = (r.regiaoCidades ?? r.RegiaoCidades ?? []).map((rc: any) => ({
      id: rc.id ?? rc.Id,
      regiaoId: rc.regiaoId ?? rc.RegiaoId,
      cidadeId: rc.cidadeId ?? rc.CidadeId,
      cidade: toCidade(rc.cidade ?? rc.Cidade)
    }));

    return {
      id: r.id ?? r.Id,
      nome: r.nome ?? r.Nome,
      ativo: r.ativo ?? r.Ativo,
      regiaoCidades: links
    };
  };

  // ---- LEITURAS
  get(id: string): Observable<Regiao> {
    return this.http.get<any>(`${this.base}/${id}`).pipe(
      map(this.normalizeRegiao)
    );
  }

  listWithCidades(): Observable<Regiao[]> {
    return this.http.get<any[]>(`${this.base}/with-cidades`).pipe(
      map(arr => arr.map(this.normalizeRegiao))
    );
  }

  create(dto: { nome: string; cidadeIds: string[] }) {
    return this.http.post<Regiao>(this.base, dto);
  }
  update(id: string, dto: { nome: string; cidadeIds: string[] }) {
    return this.http.put<Regiao>(`${this.base}/${id}`, dto);
  }
  ativar(id: string)    { return this.http.put<Regiao>(`${this.base}/${id}/ativar`, {}); }
  desativar(id: string) { return this.http.put<Regiao>(`${this.base}/${id}/desativar`, {}); }
  existsByName(nome: string, ignoreId?: string) {
    const params: any = { nome }; if (ignoreId) params.ignoreId = ignoreId;
    return this.http.get<boolean>(`${this.base}/exists`, { params });
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Regiao } from '../models/regiao';

@Injectable({ providedIn: 'root' })
export class RegiaoService {
  private base = '/api';

  constructor(private http: HttpClient) {}

  listWithCidades(): Observable<Regiao[]> {
    return this.http.get<Regiao[]>(`${this.base}/regiao/with-cidades`);
  }

  get(id: string) {
    return this.http.get<any>(`${this.base}/regiao/${id}`).pipe(
      map(r => ({
        ...r,
        regiaoCidades: (r.regiaoCidades || r.RegiaoCidades || []).map((rc: any) => ({
          ...rc,
          cidadeId: rc.cidadeId ?? rc.CidadeId
        }))
      }))
    );
  }

  create(dto: { nome: string; cidadeIds: string[] }): Observable<Regiao> {
    return this.http.post<Regiao>(`${this.base}/regiao`, dto);
  }

  update(id: string, dto: { nome: string; cidadeIds: string[] }): Observable<Regiao> {
    return this.http.put<Regiao>(`${this.base}/regiao/${id}`, dto);
  }

  existsByName(nome: string, ignoreId?: string): Observable<boolean> {
    const p = new URLSearchParams({ nome });
    if (ignoreId) p.set('ignoreId', ignoreId);
    return this.http.get<boolean>(`${this.base}/regiao/exists?` + p.toString());
  }

  ativar(id: string)     { return this.http.put(`${this.base}/regiao/${id}/ativar`, {}); }
  desativar(id: string)  { return this.http.put(`${this.base}/regiao/${id}/desativar`, {}); }
}

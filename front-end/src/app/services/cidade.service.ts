import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cidade } from '../models/cidade';

@Injectable({ providedIn: 'root' })
export class CidadeService {
  private api = '/api/cidade';
  constructor(private http: HttpClient) {}

  list(params?: { uf?: string; terms?: string }): Observable<Cidade[]> {
    return this.http.get<Cidade[]>(this.api, { params: params as any });
  }

  get(id: string): Observable<Cidade> {
    return this.http.get<Cidade>(`${this.api}/${id}`);
  }
}

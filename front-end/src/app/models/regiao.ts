import { Cidade } from './cidade';

export interface RegiaoCidade {
  id: string;
  regiaoId: string;
  cidadeId: string;
  cidade?: Cidade; // vem preenchida no /with-cidades
}

export interface Regiao {
  id: string;
  nome: string;
  ativo: boolean;
  regiaoCidades: RegiaoCidade[];
}

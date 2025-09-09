// Repositório de Regiao: acesso a dados (CRUD/consultas)
using Fretefy.Test.Domain.Entities;
using System;                               // Guid
using System.Collections.Generic;
using System.Linq;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRegiaoRepository
    {
        IQueryable<Regiao> List();                 // para compor filtros antes de executar
        Regiao Get(Guid id);                       // buscar 1 por Id
        void Add(Regiao regiao);                   // criar
        void Update(Regiao regiao);                // atualizar
        void Delete(Guid id);                      // remover por Id
        IEnumerable<Regiao> Query(string terms);   // busca por termos (ex.: nome)
        bool ExistsByName(string nome);            // <— útil p/ validar duplicidade no service
    }
}

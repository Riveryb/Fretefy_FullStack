using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Interfaces
{
    public interface IRegiaoService
    {
        Regiao Get(Guid id);
        IEnumerable<Regiao> List();
        IEnumerable<Regiao> Query(string terms);
        Regiao Create(string nome, IEnumerable<Guid> cidadeIds);  // <— valida nome + cidades
        Regiao Update(Guid id, string nome, IEnumerable<Guid> cidadeIds);
        void Delete(Guid id);
    }
}

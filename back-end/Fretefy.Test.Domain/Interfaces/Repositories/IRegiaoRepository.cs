using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRegiaoRepository : IRepository<Regiao>
    {
        Regiao GetWithCidades(Guid id);
        Regiao GetWithCidadesTracked(Guid id);
        IEnumerable<Regiao> Query(string terms);
        IEnumerable<Regiao> ListWithCidades();
        IEnumerable<Regiao> List(bool? ativo = null);
        bool ExistsByName(string nome, Guid? ignoreId = null);
    }
}

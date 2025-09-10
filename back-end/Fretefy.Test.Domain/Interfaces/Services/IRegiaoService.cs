using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Interfaces
{
    public interface IRegiaoService
    {
        // Leitura
        Regiao Get(Guid id);
        IEnumerable<Regiao> List();
        IEnumerable<Regiao> List(bool? ativo);
        IEnumerable<Regiao> ListWithCidades();   // lista já incluindo as cidades para exportação
        IEnumerable<Regiao> Query(string terms);
        bool ExistsByName(string nome, Guid? ignoreId = null);

        // Escrita
        Regiao Create(string nome, IEnumerable<Guid> cidadeIds);
        Regiao Update(Guid id, string nome, IEnumerable<Guid> cidadeIds);
        void Delete(Guid id);

        // Ativação/Desativação
        Regiao Ativar(Guid id);
        Regiao Desativar(Guid id);
    }
}

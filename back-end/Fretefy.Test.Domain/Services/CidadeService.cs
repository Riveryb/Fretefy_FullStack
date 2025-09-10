using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Fretefy.Test.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Services
{
    public class CidadeService : ICidadeService
    {
        private readonly ICidadeRepository _cidadeRepo;

        public CidadeService(ICidadeRepository cidadeRepo)
        {
            _cidadeRepo = cidadeRepo;
        }

        public Cidade Get(Guid id)
        {
            var cidade = _cidadeRepo.Get(id);
            if (cidade == null) throw new ArgumentException("Cidade não encontrada.");
            return cidade;
        }

        public IEnumerable<Cidade> List() => _cidadeRepo.List();

        public IEnumerable<Cidade> ListByUf(string uf)
        {
            if (string.IsNullOrWhiteSpace(uf))
                throw new ArgumentException("UF é obrigatória.");
            return _cidadeRepo.ListByUf(uf.Trim());
        }

        public IEnumerable<Cidade> Query(string terms)
        {
            if (string.IsNullOrWhiteSpace(terms))
                return new List<Cidade>();
            return _cidadeRepo.Query(terms.Trim());
        }
    }
}
